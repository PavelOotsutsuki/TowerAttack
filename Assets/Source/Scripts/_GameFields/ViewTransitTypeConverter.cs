using System;
using GameFields.Persons;

namespace GameFields
{
    public static class ViewTransitTypeConverter
    {
        public static ViewType GetPersonHandViewType(IPersonObject person, bool isPersonObject)
        {
            ViewType myHand = person is IPlayerObject ? ViewType.HandPlayer : ViewType.HandAI;
            ViewType enemyHand = person is IPlayerObject ? ViewType.HandAI : ViewType.HandPlayer;

            return isPersonObject ? myHand : enemyHand;
        }

        public static ViewType GetPersonTableViewType(IPersonObject person, bool isPersonObject)
        {
            ViewType myTable = person is IPlayerObject ? ViewType.TablePlayer : ViewType.TableAI;
            ViewType enemyTable = person is IPlayerObject ? ViewType.TableAI : ViewType.TablePlayer;

            return isPersonObject ? myTable : enemyTable;
        }

        public static TransitToType GetPersonHandTransitToType(IPersonObject person, bool isPersonObject)
        {
            TransitToType myHand = person is IPlayerObject ? TransitToType.HandPlayer : TransitToType.HandEnemy;
            TransitToType enemyHand = person is IPlayerObject ? TransitToType.HandEnemy : TransitToType.HandPlayer;

            return isPersonObject ? myHand : enemyHand;
        }

        public static TransitToType GetPersonFirePoolTransitToType(IPersonObject person, bool isPersonObject)
        {
            TransitToType myFirePool = person is IPlayerObject ? TransitToType.PlayerFirePool : TransitToType.EnemyFirePool;
            TransitToType enemyFirePool = person is IPlayerObject ? TransitToType.EnemyFirePool : TransitToType.PlayerFirePool;

            return isPersonObject ? myFirePool : enemyFirePool;
        }

        public static TransitFromType GetPersonHandTransitFromType(IPersonObject person, bool isPersonObject)
        {
            TransitFromType myHand = person is IPlayerObject ? TransitFromType.HandPlayer : TransitFromType.HandEnemy;
            TransitFromType enemyHand = person is IPlayerObject ? TransitFromType.HandEnemy : TransitFromType.HandPlayer;

            return isPersonObject ? myHand : enemyHand;
        }

        // ViewType
        public static ViewType ConvertToViewType(TransitToType toType)
        {
            switch (toType)
            {
                case TransitToType.Deck:
                    return ViewType.Deck;
                case TransitToType.DiscardPile:
                    return ViewType.DiscardPile;
                case TransitToType.EnemyFirePool:
                    return ViewType.FireRoot;
                case TransitToType.HandEnemy:
                    return ViewType.HandAI;
                case TransitToType.HandPlayer:
                    return ViewType.HandPlayer;
                case TransitToType.PlayerFirePool:
                    return ViewType.FireRoot;
                default:
                    throw new ArgumentOutOfRangeException($"Неизвестный {typeof(TransitToType)} для конвертрования в {typeof(ViewType)}: {toType}");
            }
        }

        public static ViewType ConvertToViewType(TransitFromType fromType)
        {
            switch (fromType)
            {
                case TransitFromType.DiscardPile:
                    return ViewType.DiscardPile;
                case TransitFromType.HandEnemy:
                    return ViewType.HandAI;
                case TransitFromType.HandPlayer:
                    return ViewType.HandPlayer;
                case TransitFromType.FireRoot:
                    return ViewType.FireRoot;
                default:
                    throw new ArgumentOutOfRangeException($"Неизвестный {typeof(TransitFromType)} для конвертрования в {typeof(ViewType)}: {fromType}");
            }
        }

        public static ViewType ConvertToViewType(TowerTransitType towerTransitType)
        {
            switch (towerTransitType)
            {
                case TowerTransitType.Deck:
                    return ViewType.Deck;
                default:
                    throw new ArgumentOutOfRangeException($"Неизвестный {typeof(TowerTransitType)} для конвертрования в {typeof(ViewType)}: {towerTransitType}");
            }
        }


        public static ViewType ConvertToViewTypeByPerson(TowerTransitType towerTransitType, IPersonObject person, bool isPersonObject)
        {
            switch (towerTransitType)
            {
                case TowerTransitType.Hand:
                    ViewType myHand = person is IPlayerObject ? ViewType.HandPlayer : ViewType.HandAI;
                    ViewType enemyHand = person is IPlayerObject ? ViewType.HandAI : ViewType.HandPlayer;
                    return isPersonObject ? myHand : enemyHand;
                default:
                    throw new ArgumentOutOfRangeException($"Неизвестный {typeof(TowerTransitType)} для конвертрования в {typeof(ViewType)}: {towerTransitType}");
            }
        }

        // TransitToType
        public static TransitToType ConvertToTransitToType(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Deck:
                    return TransitToType.Deck;
                case ViewType.DiscardPile:
                    return TransitToType.DiscardPile;
                case ViewType.HandAI:
                    return TransitToType.HandEnemy;
                case ViewType.HandPlayer:
                    return TransitToType.HandPlayer;
                default:
                    throw new ArgumentOutOfRangeException($"Неизвестный {typeof(ViewType)} для конвертрования в {typeof(TransitToType)}: {viewType}");
            }
        }

        public static TransitToType ConvertToTransitToType(TransitFromType fromType)
        {
            switch (fromType)
            {
                case TransitFromType.DiscardPile:
                    return TransitToType.DiscardPile;
                case TransitFromType.HandEnemy:
                    return TransitToType.HandEnemy;
                case TransitFromType.HandPlayer:
                    return TransitToType.HandPlayer;
                default:
                    throw new ArgumentOutOfRangeException($"Неизвестный {typeof(TransitFromType)} для конвертрования в {typeof(TransitToType)}: {fromType}");
            }
        }

        public static TransitToType ConvertToTransitToType(TowerTransitType towerTransitType)
        {
            switch (towerTransitType)
            {
                case TowerTransitType.Deck:
                    return TransitToType.Deck;
                default:
                    throw new ArgumentOutOfRangeException($"Неизвестный {typeof(TowerTransitType)} для конвертрования в {typeof(TransitToType)}: {towerTransitType}");
            }
        }

        // TransitFromType
        public static TransitFromType ConvertToTransitFromType(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.DiscardPile:
                    return TransitFromType.DiscardPile;
                case ViewType.HandAI:
                    return TransitFromType.HandEnemy;
                case ViewType.HandPlayer:
                    return TransitFromType.HandPlayer;
                case ViewType.FireRoot:
                    return TransitFromType.FireRoot;
                default:
                    throw new ArgumentOutOfRangeException($"Неизвестный {typeof(ViewType)} для конвертрования в {typeof(TransitFromType)}: {viewType}");
            }
        }

        public static TransitFromType ConvertToTransitFromType(TransitToType toType)
        {
            switch (toType)
            {
                case TransitToType.DiscardPile:
                    return TransitFromType.DiscardPile;
                case TransitToType.HandEnemy:
                    return TransitFromType.HandEnemy;
                case TransitToType.HandPlayer:
                    return TransitFromType.HandPlayer;
                case TransitToType.EnemyFirePool:
                    return TransitFromType.FireRoot;
                case TransitToType.PlayerFirePool:
                    return TransitFromType.FireRoot;
                default:
                    throw new ArgumentOutOfRangeException($"Неизвестный {typeof(TransitToType)} для конвертрования в {typeof(TransitFromType)}: {toType}");
            }
        }

        // TowerTransitType
        public static TowerTransitType ConvertToTowerTransitType(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Deck:
                    return TowerTransitType.Deck;
                default:
                    throw new ArgumentOutOfRangeException($"Неизвестный {typeof(ViewType)} для конвертрования в {typeof(TowerTransitType)}: {viewType}");
            }
        }

        public static TowerTransitType ConvertToTowerTransitType(TransitToType toType)
        {
            switch (toType)
            {
                case TransitToType.Deck:
                    return TowerTransitType.Deck;
                default:
                    throw new ArgumentOutOfRangeException($"Неизвестный {typeof(TransitToType)} для конвертрования в {typeof(TowerTransitType)}: {toType}");
            }
        }
    }
}
