using System.Collections.Generic;
using Tools;

namespace GameFields.LightControls
{
    public class LightControllerActivateData : IData
    {
        private readonly IEnumerable<LightableObject> _lightableObjects;

        public LightControllerActivateData(IEnumerable<LightableObject> lightableObjects)
        {
            _lightableObjects = lightableObjects;
        }

        public IEnumerable<LightableObject> LightableObjects => _lightableObjects;
    }
}