using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using UnityEngine;

namespace GameFields
{
    public class StoneFrame : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private Stone[] _stoneVariants;
        [SerializeField] private RectTransform _targetTransform;

        private ReadOnlyRectTransform _ROTransform;
        private float _thisHeight;
        private float _thisWidth;

        //private readonly List<Stone> _points = new List<Stone>();
        //private readonly Dictionary<Stone, Vector2> _stonesInfo = new Dictionary<Stone, Vector2>();

        private int CountVariants => _stoneVariants.Length;

        public void Init()
        {
            _ROTransform = new ReadOnlyRectTransform(_targetTransform);

            _thisHeight = _ROTransform.GetHeight();
            _thisWidth = _ROTransform.GetWidth();

            //foreach (Stone stone in _stoneVariants)
            //{
            //    ReadOnlyRectTransform stoneTransform = new ReadOnlyRectTransform((RectTransform)stone.transform);
            //    _stonesInfo.Add(stone, stoneTransform.GetRect());
            //}

            Vector2 leftUpPoint = new Vector2(_thisWidth / 2f * (-1f), _thisHeight / 2f);
            Vector2 leftDownPoint = new Vector2(_thisWidth / 2f * (-1f), _thisHeight / 2f * (-1f));
            Vector2 rightDownPoint = new Vector2(_thisWidth / 2f, _thisHeight / 2f * (-1f));
            Vector2 rightUpPoint = new Vector2(_thisWidth / 2f, _thisHeight / 2f);

            SetPoints(leftUpPoint, leftDownPoint);
            SetPoints(leftDownPoint, rightDownPoint);
            SetPoints(rightDownPoint, rightUpPoint);
            SetPoints(rightUpPoint, leftUpPoint);

            Deactivate();
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void SetPoints(Vector2 startPosition, Vector2 endPosition)
        {
            const float StoneWidth = 50f;
            const float StoneHeight = 50f;
            const float StoneWidthMaxOffset = 4f;
            const float StoneHeightMaxOffset = 4f;
            Vector3 scaleVector = new Vector3(1f, 1f, 1f);

            if (startPosition == endPosition)
                return;

            Vector2 currentPosition = startPosition;
            Vector2 direction = endPosition - startPosition;
            float directionVectorX = direction.x < 0 ? -1 : 1;
            float directionVectorY = direction.y < 0 ? -1 : 1;
            float? verticalFactor = Mathf.Approximately(direction.x, 0f) ? null : direction.y / direction.x;
            float? horizontalFactor = Mathf.Approximately(direction.y, 0f) ? null : direction.x / direction.y;

            while (
                (Mathf.Abs(direction.x) > Mathf.Abs(currentPosition.x - startPosition.x) || Mathf.Approximately(direction.x, 0f)) &&
                (Mathf.Abs(direction.y) > Mathf.Abs(currentPosition.y - startPosition.y) || Mathf.Approximately(direction.y, 0f))
                )
            {
                Stone template = _stoneVariants[Random.Range(0, CountVariants)];

                Stone stone = Instantiate(template, _targetTransform);
                //_points.Add(stone);

                float offsetX = Random.Range(StoneWidthMaxOffset * (-1f), StoneWidthMaxOffset);
                float offsetY = Random.Range(StoneHeightMaxOffset * (-1f), StoneHeightMaxOffset);

                Vector2 stoneSize = new Vector2(StoneWidth + offsetX, StoneHeight + offsetY);

                RectTransform stoneRectTransform = (RectTransform)stone.transform;
                ReadOnlyRectTransform ROStoneTransform = new ReadOnlyRectTransform(stoneRectTransform);
                Movement stoneMovement = new Movement(stoneRectTransform);

                ROStoneTransform.SetSize(stoneSize);

                float rotationZ = Random.Range(-180f, 180f);
                Vector3 rotation = new Vector3(0f, 0f, rotationZ);

                stoneMovement.MoveLocalInstantly(currentPosition, rotation, scaleVector);

                float verticalStep = stoneSize.y * 0.8f;
                float horizontalStep = stoneSize.x * 0.8f;

                float stepY;

                if (verticalFactor == null)
                {
                    stepY = verticalStep;
                }
                else
                {
                    stepY = verticalStep - verticalStep / (verticalFactor.Value * verticalFactor.Value + 1);
                }

                float stepX;

                if (horizontalFactor == null)
                {
                    stepX = horizontalStep;
                }
                else
                {
                    stepX = horizontalStep - horizontalStep / (horizontalFactor.Value * horizontalFactor.Value + 1);
                }

                stepX *= directionVectorX;
                stepY *= directionVectorY;

                Vector2 step = new Vector2(stepX, stepY);
                currentPosition += step; 
            }


            //Debug.Log("startPosition: " + startPosition);
            //Debug.Log("endPosition: " + endPosition);
            //Debug.Log("direction: " + direction);
            //Debug.Log("distance: " + distance);

            //Stone template = _stoneVariants[Random.Range(0, CountVariants)];

            //for (float d = 0; d <= distance; d += _stonesInfo[template].)
            //{

            //    template = _stoneVariants[Random.Range(0, CountVariants)];

            //    Stone stone = Instantiate(template, _targetTransform);
            //    _points.Add(stone);
            //}
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(StoneFrame))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _targetTransform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}