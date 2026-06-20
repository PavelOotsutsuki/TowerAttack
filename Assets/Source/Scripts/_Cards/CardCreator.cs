using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CardCreator : MonoBehaviour
    {
        [SerializeField] private Card _template_1_Zhyzha;
        [SerializeField] private Card _template_2_Greedy;
        [SerializeField] private Card _template_3_Pyromancer;
        [SerializeField] private Card _template_4_CoolBookmaker;
        [SerializeField] private Card _template_5_BlindOldMan;
        [SerializeField] private Card _template_6_DetectiveRhodes;
        [SerializeField] private Card _template_7_BlueGnome;
        [SerializeField] private Card _template_8_TimeLord;
        [SerializeField] private Card _template_9_ThreeGuys;
        [SerializeField] private Card _template_10_TimeMistress;
        [SerializeField] private Card _template_11_SharpSnake;
        [SerializeField] private Card _template_12_ImpArmy;
        [SerializeField] private Card _template_13_CursedMark;
        [SerializeField] private Card _template_14_RushingMailman;
        [SerializeField] private Card _template_15_Schemer;
        [SerializeField] private Card _template_16_Mime;
        [SerializeField] private Card _template_17_RedGnome;
        [SerializeField] private Card _template_18_TimeChild;
        [SerializeField] private Card _template_19_Undergrounder;
        [SerializeField] private Card _template_20_RobinGood;
        [SerializeField] private Card _template_21_General;
        [SerializeField] private Card _template_22_FateMistress;
        [SerializeField] private Card _template_23_DumbMonk;
        [SerializeField] private Card _template_24_LeftEyedSister;
        [SerializeField] private Card _template_25_JusticeBull;
        [SerializeField] private Card _template_26_PatriarchCorall;
        [SerializeField] private Card _template_27_GreenGnome;
        [SerializeField] private Card _template_28_LittleBrother;
        [SerializeField] private Card _template_29_BrothersMother;
        [SerializeField] private Card _template_30_Scarecrow;
        [SerializeField] private Card _template_31_LuckyHorseshoe;
        [SerializeField] private Card _template_32_WiseMonk;
        [SerializeField] private Card _template_33_CowsHerd;
        [SerializeField] private Card _template_34_HungryOgre;
        [SerializeField] private Card _template_35_Sharper;
        [SerializeField] private Card _template_36_Gunner;
        [SerializeField] private Card _template_37_WhiteGnome;
        [SerializeField] private Card _template_38_MiddleBrother;
        [SerializeField] private Card _template_39_DeadOgre;
        [SerializeField] private Card _template_40_OutOfControlBus;
        [SerializeField] private Card _template_41_CursedMailman;
        [SerializeField] private Card _template_42_RightEyedSister;
        [SerializeField] private Card _template_43_StrongOgre;
        [SerializeField] private Card _template_44_MafiaBoss;
        [SerializeField] private Card _template_45_PyromancersManuscript;
        [SerializeField] private Card _template_46_FalsePrince;
        [SerializeField] private Card _template_47_BlackGnome;
        [SerializeField] private Card _template_48_BigBrother;
        [SerializeField] private Card _template_49_LastChance;
        [SerializeField] private Card _template_50_FallenGuardian;

        private readonly Dictionary<CardName, Card> _createPairs = new Dictionary<CardName, Card>();

        public void Init()
        {
            _createPairs.Add(CardName.Zhyzha, _template_1_Zhyzha);
            _createPairs.Add(CardName.Greedy, _template_2_Greedy);
            _createPairs.Add(CardName.Pyromancer, _template_3_Pyromancer);
            _createPairs.Add(CardName.CoolBookmaker, _template_4_CoolBookmaker);
            _createPairs.Add(CardName.BlindOldMan, _template_5_BlindOldMan);
            _createPairs.Add(CardName.DetectiveRhodes, _template_6_DetectiveRhodes);
            _createPairs.Add(CardName.BlueGnome, _template_7_BlueGnome);
            _createPairs.Add(CardName.TimeLord, _template_8_TimeLord);
            _createPairs.Add(CardName.ThreeGuys, _template_9_ThreeGuys);
            _createPairs.Add(CardName.TimeMistress, _template_10_TimeMistress);
            _createPairs.Add(CardName.SharpSnake, _template_11_SharpSnake);
            _createPairs.Add(CardName.ImpArmy, _template_12_ImpArmy);
            _createPairs.Add(CardName.CursedMark, _template_13_CursedMark);
            _createPairs.Add(CardName.RushingMailman, _template_14_RushingMailman);
            _createPairs.Add(CardName.Schemer, _template_15_Schemer);
            _createPairs.Add(CardName.Mime, _template_16_Mime);
            _createPairs.Add(CardName.RedGnome, _template_17_RedGnome);
            _createPairs.Add(CardName.TimeChild, _template_18_TimeChild);
            _createPairs.Add(CardName.Undergrounder, _template_19_Undergrounder);
            _createPairs.Add(CardName.RobinGood, _template_20_RobinGood);
            _createPairs.Add(CardName.General, _template_21_General);
            _createPairs.Add(CardName.FateMistress, _template_22_FateMistress);
            _createPairs.Add(CardName.DumbMonk, _template_23_DumbMonk);
            _createPairs.Add(CardName.LeftEyedSister, _template_24_LeftEyedSister);
            _createPairs.Add(CardName.JusticeBull, _template_25_JusticeBull);
            _createPairs.Add(CardName.PatriarchCorall, _template_26_PatriarchCorall);
            _createPairs.Add(CardName.GreenGnome, _template_27_GreenGnome);
            _createPairs.Add(CardName.LittleBrother, _template_28_LittleBrother);
            _createPairs.Add(CardName.BrothersMother, _template_29_BrothersMother);
            _createPairs.Add(CardName.Scarecrow, _template_30_Scarecrow);
            _createPairs.Add(CardName.LuckyHorseshoe, _template_31_LuckyHorseshoe);
            _createPairs.Add(CardName.WiseMonk, _template_32_WiseMonk);
            _createPairs.Add(CardName.CowsHerd, _template_33_CowsHerd);
            _createPairs.Add(CardName.HungryOgre, _template_34_HungryOgre);
            _createPairs.Add(CardName.Sharper, _template_35_Sharper);
            _createPairs.Add(CardName.Gunner, _template_36_Gunner);
            _createPairs.Add(CardName.WhiteGnome, _template_37_WhiteGnome);
            _createPairs.Add(CardName.MiddleBrother, _template_38_MiddleBrother);
            _createPairs.Add(CardName.DeadOgre, _template_39_DeadOgre);
            _createPairs.Add(CardName.OutOfControlBus, _template_40_OutOfControlBus);
            _createPairs.Add(CardName.CursedMailman, _template_41_CursedMailman);
            _createPairs.Add(CardName.RightEyedSister, _template_42_RightEyedSister);
            _createPairs.Add(CardName.StrongOgre, _template_43_StrongOgre);
            _createPairs.Add(CardName.MafiaBoss, _template_44_MafiaBoss);
            _createPairs.Add(CardName.PyromancersManuscript, _template_45_PyromancersManuscript);
            _createPairs.Add(CardName.FalsePrince, _template_46_FalsePrince);
            _createPairs.Add(CardName.BlackGnome, _template_47_BlackGnome);
            _createPairs.Add(CardName.BigBrother, _template_48_BigBrother);
            _createPairs.Add(CardName.LastChance, _template_49_LastChance);
            _createPairs.Add(CardName.FallenGuardian, _template_50_FallenGuardian);
        }

        public Card CreateInstantly(CardName cardName, Transform parent)
        {
            Card template = _createPairs[cardName];

            Card createdCard = Instantiate(template, parent);

            return createdCard;
        }

        public Card CreateInstantly(CardName cardName)
        {
            Card template = _createPairs[cardName];

            Card createdCard = Instantiate(template);

            return createdCard;
        }
    }
}