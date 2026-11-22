using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CardCreator : MonoBehaviour
    {
        [SerializeField] private Card _template_1_Zhyzha;
        [SerializeField] private Card _template_2_Greedy;
        [SerializeField] private Card _template_3_Pyromancer;
        [SerializeField] private Card _template_4_CoolBookie;
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
        [SerializeField] private Card _template_23_FoolishMonk;
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
        private readonly Dictionary<CardName, string> _audioUrls = new Dictionary<CardName, string>();

        public void Init()
        {
            _createPairs.Add(CardName.Zhyzha, _template_1_Zhyzha);
            _createPairs.Add(CardName.Greedy, _template_2_Greedy);
            _createPairs.Add(CardName.Pyromancer, _template_3_Pyromancer);
            _createPairs.Add(CardName.CoolBookie, _template_4_CoolBookie);
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
            _createPairs.Add(CardName.FoolishMonk, _template_23_FoolishMonk);
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

//             "1/Zhyzha_1"
// "2/Greedy_2"
// "3/Pyromancer_3"
// "4/CoolBookie_4"
// "5/BlindOldMan_5"
// "6/DetectiveRhodes_6"
// "7/BlueGnome_7"
// "8/TimeLord_8"
// "9/ThreeGuys_9"
// "10/TimeMistress_10"
// "11/SharpSnake_11"
// "12/ImpArmy_12"
// "13/CursedMark_13"
// "14/RushingMailman_14"
// "15/Schemer_15"
// "16/Mime_16"
// "17/RedGnome_17"
// "18/TimeChild_18"
// "19/Undergrounder_19"
// "20/RobinGood_20"
// "21/General_21"
// "22/FateMistress_22"
// "23/FoolishMonk_23"
// "24/LeftEyedSister_24"
// "25/JusticeBull_25"
// "26/PatriarchCorall_26"
// "27/GreenGnome_27"
// "28/LittleBrother_28"
// "29/BrothersMother_29"
// "30/Scarecrow_30"
// "31/LuckyHorseshoe_31"
// "32/WiseMonk_32"
// "33/CowsHerd_33"
// "34/HungryOgre_34"
// "35/Sharper_35"
// "36/Gunner_36"
// "37/WhiteGnome_37"
// "38/MiddleBrother_38"
// "39/DeadOgre_39"
// "40/OutOfControlBus_40"
// "41/CursedMailman_41"
// "42/RightEyedSister_42"
// "43/StrongOgre_43"
// "44/MafiaBoss_44"
// "45/PyromancersManuscript_45"
// "46/FalsePrince_46"
// "47/BlackGnome_47"
// "48/BigBrother_48"
// "49/LastChance_49"
// "50/FallenGuardian_50"

            _audioUrls.Add(CardName.Zhyzha, "1/Zhyzha_1");
            _audioUrls.Add(CardName.Greedy, "2/Greedy_2");
            _audioUrls.Add(CardName.Pyromancer, "3/Pyromancer_3");
            _audioUrls.Add(CardName.CoolBookie, "4/CoolBookie_4");
            _audioUrls.Add(CardName.BlindOldMan, "5/BlindOldMan_5");
            _audioUrls.Add(CardName.DetectiveRhodes, "6/DetectiveRhodes_6");
            _audioUrls.Add(CardName.BlueGnome, "7/BlueGnome_7");
            _audioUrls.Add(CardName.TimeLord, "3/Pyromancer_3");
            _audioUrls.Add(CardName.ThreeGuys, "3/Pyromancer_3");
            _audioUrls.Add(CardName.TimeMistress, "3/Pyromancer_3");
            _audioUrls.Add(CardName.SharpSnake, "3/Pyromancer_3");
            _audioUrls.Add(CardName.ImpArmy, "3/Pyromancer_3");
            _audioUrls.Add(CardName.CursedMark, "3/Pyromancer_3");
            _audioUrls.Add(CardName.RushingMailman, "3/Pyromancer_3");
            _audioUrls.Add(CardName.Schemer, "3/Pyromancer_3");
            _audioUrls.Add(CardName.Mime, "3/Pyromancer_3");
            _audioUrls.Add(CardName.RedGnome, "3/Pyromancer_3");
            _audioUrls.Add(CardName.TimeChild, "3/Pyromancer_3");
            _audioUrls.Add(CardName.Undergrounder, "3/Pyromancer_3");
            _audioUrls.Add(CardName.RobinGood, "3/Pyromancer_3");
            _audioUrls.Add(CardName.General, "3/Pyromancer_3");
            _audioUrls.Add(CardName.FateMistress, "3/Pyromancer_3");
            _audioUrls.Add(CardName.FoolishMonk, "3/Pyromancer_3");
            _audioUrls.Add(CardName.LeftEyedSister, "3/Pyromancer_3");
            _audioUrls.Add(CardName.JusticeBull, "3/Pyromancer_3");
            _audioUrls.Add(CardName.PatriarchCorall, "3/Pyromancer_3");
            _audioUrls.Add(CardName.GreenGnome, "3/Pyromancer_3");
            _audioUrls.Add(CardName.LittleBrother, "3/Pyromancer_3");
            _audioUrls.Add(CardName.BrothersMother, "3/Pyromancer_3");
            _audioUrls.Add(CardName.Scarecrow, "3/Pyromancer_3");
            _audioUrls.Add(CardName.LuckyHorseshoe, "3/Pyromancer_3");
            _audioUrls.Add(CardName.WiseMonk, "3/Pyromancer_3");
            _audioUrls.Add(CardName.CowsHerd, "3/Pyromancer_3");
            _audioUrls.Add(CardName.HungryOgre, "3/Pyromancer_3");
            _audioUrls.Add(CardName.Sharper, "3/Pyromancer_3");
            _audioUrls.Add(CardName.Gunner, "3/Pyromancer_3");
            _audioUrls.Add(CardName.WhiteGnome, "3/Pyromancer_3");
            _audioUrls.Add(CardName.MiddleBrother, "3/Pyromancer_3");
            _audioUrls.Add(CardName.DeadOgre, "3/Pyromancer_3");
            _audioUrls.Add(CardName.OutOfControlBus, "3/Pyromancer_3");
            _audioUrls.Add(CardName.CursedMailman, "3/Pyromancer_3");
            _audioUrls.Add(CardName.RightEyedSister, "3/Pyromancer_3");
            _audioUrls.Add(CardName.StrongOgre, "3/Pyromancer_3");
            _audioUrls.Add(CardName.MafiaBoss, "3/Pyromancer_3");
            _audioUrls.Add(CardName.PyromancersManuscript, "3/Pyromancer_3");
            _audioUrls.Add(CardName.FalsePrince, "3/Pyromancer_3");
            _audioUrls.Add(CardName.BlackGnome, "3/Pyromancer_3");
            _audioUrls.Add(CardName.BigBrother, "3/Pyromancer_3");
            _audioUrls.Add(CardName.LastChance, "3/Pyromancer_3");
            _audioUrls.Add(CardName.FallenGuardian, "3/Pyromancer_3");
        }

        public Card CreateInstantly(CardName cardName, Transform parent)
        {
            Card template = _createPairs[cardName];

            Card createdCard = Instantiate(template, parent);

            return createdCard;
        }

        public string GetAudioUrl(CardName cardName)
        {
            return _audioUrls[cardName];
        }

        public Card CreateInstantly(CardName cardName)
        {
            Card template = _createPairs[cardName];

            Card createdCard = Instantiate(template);

            return createdCard;
        }
    }
}