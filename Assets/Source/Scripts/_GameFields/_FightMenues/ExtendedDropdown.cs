using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.FightMenues
{
    public class ExtendedDropdown : TMP_Dropdown
    {
        private ScrollRect _scrollRect; // Сериализовнно почему-то не выводит.
        // Тк все равно ебучий Dropdown создает каждый раз объект на основе template вместо того чтобы просто брать его. Нахуя...

        private ExtendedToggle[] _currentToggles;
       
        private void Update()
        {
            if (IsExpanded)
            {
                if (_scrollRect == null)
                {
                    _scrollRect = GetComponentInChildren<ScrollRect>();
                    _currentToggles = GetComponentsInChildren<ExtendedToggle>();

                    Transform currentToggle = _scrollRect.content.GetChild(this.value + 1);

                    foreach (ExtendedToggle toggle in _currentToggles)
                    {
                        toggle.Init(Scroll);

                        if (toggle.transform == currentToggle)
                        {
                            toggle.Select();
                            Scroll(currentToggle as RectTransform);
                        }
                    }
                }
            }
        }

        public void Scroll(RectTransform item)
        {
            if (item == null)
                return;

            int index;
            for (index = 0; index < _scrollRect.content.childCount; index++) // Находим какой по счету item
            {
                if (_scrollRect.content.GetChild(index) == item)
                    break;
            }

            index++; // делаем индекс на 1 больше, чтобы фокус держался на середине

            if (index > _scrollRect.content.childCount)
                index = _scrollRect.content.childCount;

            // Вычисляем позицию
            float heightViewForYouCurrent = (_scrollRect.gameObject.transform as RectTransform).rect.height; // Вычисляем сколько пользователь видит
            float step = item.rect.height; // Вычисляем сколько нужно на один элемент (у всех item height одинаковый)

            float scrollPositionY = step * index - heightViewForYouCurrent;

            if (scrollPositionY < 0)
                scrollPositionY = 0;

            _scrollRect.content.localPosition = new Vector3(_scrollRect.content.localPosition.x, scrollPositionY, _scrollRect.content.localPosition.z);
        }
    }
}