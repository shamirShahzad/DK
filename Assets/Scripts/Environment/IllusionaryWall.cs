using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class IllusionaryWall : MonoBehaviour
    {
        public bool wallhasBeenHit;
        public Material illusionaryWallMaterial;
        public float alpha;
        public float fadeTimer = 2.5f;
        public BoxCollider wallCollider;
        [SerializeField]
        MeshRenderer wallMesh;

        public AudioSource audioSource;
        public AudioClip illusionaryWallSound;

        private void Update()
        {
            if (wallhasBeenHit)
            {
                FadeIllusionaryWall();
            }
        }
        public void FadeIllusionaryWall()
        {
            Material wallMat = Instantiate(illusionaryWallMaterial);
            alpha = illusionaryWallMaterial.color.a;
            alpha = alpha - Time.deltaTime / fadeTimer;
            Color fadedWallColor = new Color(1, 1, 1, alpha);
            illusionaryWallMaterial.color = fadedWallColor;

            if (wallCollider.enabled)
            {
                wallCollider.enabled = false;
                audioSource.PlayOneShot(illusionaryWallSound);
            }

            if (alpha <= 0)
            {
                wallMesh.enabled = false;
                illusionaryWallMaterial.color = wallMat.color;
                Destroy(this);
            }
        }
    }
}
