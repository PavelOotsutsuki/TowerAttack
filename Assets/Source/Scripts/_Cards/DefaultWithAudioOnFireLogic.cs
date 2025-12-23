using System.Collections;
using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    //[RequireComponent(typeof(AudioSource))]
    public class DefaultWithAudioOnFireLogic : DefaultOnFireLogic
    {
        //[SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _fireSound;

        //public override void Init()
        //{
        //    base.Init();

        //    //_audioSource.clip = _fireSound;
        //}

        public override void Activate(OnFireLogicActivateData data)
        {
            AudioSource.PlayClipAtPoint(_fireSound, Vector3.zero);

            base.Activate(data);
        }

        //#region AutomaticFillComponents
        //[ContextMenu(nameof(DefineAllComponents) + nameof(DefaultWithAudioOnFireLogic))]
        //public override List<ComponentAttachInfo> DefineAllComponents()
        //{
        //    List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
        //    {
        //        DefineAudioSource(),
        //    };

        //    list.AddRange(base.DefineAllComponents());

        //    return list;
        //}

        //[ContextMenu(nameof(DefineAudioSource))]
        //private ComponentAttachInfo DefineAudioSource()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _audioSource, ComponentLocationTypes.InThis);
        //}
        //#endregion 
    }
}
