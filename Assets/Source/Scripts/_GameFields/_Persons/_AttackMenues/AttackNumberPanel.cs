using System.Collections.Generic;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    [RequireComponent(typeof(FadablePanel))]
    public abstract class AttackNumberPanel : MonoBehaviour, ICompletable, IWorkable<AttackNumberPanelActivateData>, IAutomaticFillComponents
    {
        public abstract bool IsComplete { get; }

        public abstract bool? IsActive { get; protected set; }

        public abstract void Activate(AttackNumberPanelActivateData data);
        public abstract void Deactivate();

        public abstract List<ComponentAttachInfo> DefineAllComponents();
    }
}