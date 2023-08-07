using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace terrain
{
    public class TerrainManagerMB : MonoBehaviour
    {
        //------------------------|| ATTRIBUTES ||---------------------------//

        public ITerrainManager terrainManager { get; set; }
        public bool displayWireframe;


        //--------------------------|| METHODS ||----------------------------//

        // Start is called before the first frame update
        void Start()
        {
            if (terrainManager != null)
            {
                //Hide editor terrain and create game terrain
                Transform editorTerrain = this.transform.Find("editor terrain");
                if (editorTerrain != null)
                {
                    editorTerrain.gameObject.SetActive(false);
                }
                terrainManager.GenerateChunks(this.gameObject, "game terrain");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

