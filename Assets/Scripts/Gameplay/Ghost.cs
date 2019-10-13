using System.Linq;
using UnityEngine;

namespace URacing
{
    public class Ghost : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _meshRenderers = default;

        private static readonly int Mode = Shader.PropertyToID("_Mode");

        public void Init()
        {
            foreach (var material in _meshRenderers.SelectMany(meshRenderer => meshRenderer.materials))
            {
                material.SetFloat(Mode, 3.0f);

                var color = material.color;
                color.a = .2f;

                material.color = color;
            }
        }
    }
}