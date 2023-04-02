using System;
using System.Collections.Generic;
using GameModels.DataStructs;

namespace WorldServerPlugin.Game
{
    public class AOIManager
    {
        public float CellSize { get; }
        private readonly Dictionary<Vector2Int, HashSet<Entity>> grid;
        private readonly object gridLock = new object();
        private GridBounds gridBounds;

        public AOIManager(float cellSize)
        {
            CellSize = cellSize;
            grid = new Dictionary<Vector2Int, HashSet<Entity>>();
            gridBounds = new GridBounds(0, 0, 0, 0);
        }

        public void AddEntity(Entity newEntity)
        {
            Vector2Int cellCoords = WorldToCellCoords(newEntity.PositionX, newEntity.PositionY);
            lock(gridLock)
            {
                if(!grid.ContainsKey(cellCoords))
                {
                    grid[cellCoords] = new HashSet<Entity>();
                }
                grid[cellCoords].Add(newEntity);
                UpdateGridBounds();
            }
        }

        public void RemoveEntity(Entity entity)
        {
            Vector2Int cellCoords = WorldToCellCoords(entity.PositionX, entity.PositionY);
            lock(gridLock) 
            {
                if (grid.ContainsKey(cellCoords))
                {
                    grid[cellCoords].Remove(entity);

                    if (grid[cellCoords].Count == 0)
                    {
                        grid.Remove(cellCoords);
                    }
                    UpdateGridBounds();
                }
            }
        }
        public void UpdateEntity(Entity entity, float newPositionX, float newPositionY)
        {
            Vector2Int oldCellCoords = WorldToCellCoords(entity.PositionX, entity.PositionY);
            Vector2Int newCellCoords = WorldToCellCoords(newPositionX, newPositionY);

            if (oldCellCoords != newCellCoords)
            {
                lock (gridLock)
                {
                    RemoveEntity(entity);
                    entity.PositionX = newPositionX;
                    entity.PositionY = newPositionY;
                    AddEntity(entity);
                }
            }
            else
            {
                entity.PositionX = newPositionX;
                entity.PositionY = newPositionY;
            }
        }
        public Dictionary<Vector2Int, List<int>> GetGridDataWithEntityIDs()
        {
            Dictionary<Vector2Int, List<int>> gridData = new Dictionary<Vector2Int, List<int>>();

            foreach (var cell in grid)
            {
                List<int> entityIds = new List<int>();

                foreach (Player entity in cell.Value)
                {
                    entityIds.Add((entity.PlayerID));
                }

                gridData[cell.Key] = entityIds;
            }

            return gridData;
        }
        public HashSet<Entity> GetEntitiesInAreaOfInterest(Entity entity, int radius)
        {
            HashSet<Entity> entitiesInArea = new HashSet<Entity>();
            Vector2Int entityCellCoords = WorldToCellCoords(entity.PositionX, entity.PositionY);

            lock (gridLock)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    for (int y = -radius; y <= radius; y++)
                    {
                        Vector2Int currentCell = entityCellCoords + new Vector2Int(x, y);
                        if (grid.TryGetValue(currentCell, out HashSet<Entity> cellEntities))
                        {
                            entitiesInArea.UnionWith(cellEntities);
                        }
                    }
                }
            }

            return entitiesInArea;
        }
        private void UpdateGridBounds()
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach (var cellCoords in grid.Keys)
            {
                minX = Math.Min(minX, cellCoords.X);
                minY = Math.Min(minY, cellCoords.Y);
                maxX = Math.Max(maxX, cellCoords.X);
                maxY = Math.Max(maxY, cellCoords.Y);
            }

            float xMin = minX * CellSize;
            float yMin = minY * CellSize;
            float width = (maxX - minX + 1) * CellSize;
            float height = (maxY - minY + 1) * CellSize;

            gridBounds = new GridBounds(xMin, yMin, width, height);
        }
        public GridBounds GetGridBounds()
        {
            lock (gridLock)
            {
                return gridBounds;
            }
        }

        public SerializableGridBounds GetSerializableGridBounds()
        {
            GridBounds bounds = GetGridBounds();

            return new SerializableGridBounds
            {
                MinX = (int)bounds.XMin,
                MinY = (int)bounds.YMin,
                MaxX = (int)bounds.XMax,
                MaxY = (int)bounds.YMax,
            };
        }
        public void ClearGrid()
        {
            lock (gridLock)
            {
                grid.Clear();
            }
        }

        private Vector2Int WorldToCellCoords(float positionX, float positionY)
        {
            int x = (int)Math.Floor(positionX / CellSize);
            int y = (int)Math.Floor(positionY / CellSize);
            return new Vector2Int(x, y);
        }
    }
}
