using System.Collections.Generic;
using System.Threading;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Views.BigCardViews.CardDescriptions
{
    [RequireComponent(typeof(FadableLabel))]
    public class CardDescription : MonoBehaviour, IViewable<CardDescriptionActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private FadableLabel _fadableLabel;
        [SerializeField] private Image _descriptionImage;
        [SerializeField] private Outline _outline;

        private CancellationToken _currentToken;

        public bool? IsShown { get; private set; } = false;

        public void Init()
        {
            _outline.enabled = false;
            gameObject.SetActive(true);

            _fadableLabel.Init();
        }

        public void Show(CardDescriptionActivateData data)
        {
            if (IsShown == true)
                return;

            IsShown = true;

            _currentToken = data.Token;
            _descriptionImage.color = data.ActivateColor;
            LabelActivateDataAsync fadableLabelActivateData = new LabelActivateDataAsync(new LabelActivateData(data.Description), _currentToken);
            _outline.enabled = data.IsOutline;

            //Debug.Log("CardDescription message: " + data.Description);
            _fadableLabel.Show(fadableLabelActivateData);
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;

            _fadableLabel.Hide(new CancellationTokenData(_currentToken));
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CardDescription))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFadableLabel(),
                DefineImage(),
                DefineOutline()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFadableLabel))]
        private ComponentAttachInfo DefineFadableLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadableLabel, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineImage))]
        private ComponentAttachInfo DefineImage()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _descriptionImage, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineOutline))]
        private ComponentAttachInfo DefineOutline()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _outline, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}