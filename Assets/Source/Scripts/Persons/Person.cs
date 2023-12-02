using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persons
{
    public abstract class Person : MonoBehaviour
    {
        [SerializeField] private int _countDrawCards = 1;

        public int CountDrawCards => _countDrawCards;

        public void Init()
        {

        }
    }
}
