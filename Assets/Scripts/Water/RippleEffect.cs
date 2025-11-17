using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RippleEffect : MonoBehaviour
{
    public int TextureSize = 1024;
    public RenderTexture ObjectsRT;
    private RenderTexture CurrRT, PrevRT, TempRT;
    public Shader RippleShader, AddShader, ScrollShader;
    private Material RippleMat, AddMat, ScrollMat;

    private Vector3 prevLocation;
    private float rippleWorldSize = 275f;


    [SerializeField] Transform targetTranform;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        //Creating render textures and materials
        CurrRT = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.RHalf);
        CurrRT.filterMode = FilterMode.Bilinear;
        PrevRT = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.RHalf);
        PrevRT.filterMode = FilterMode.Bilinear;
        TempRT = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.RHalf);
        TempRT.filterMode = FilterMode.Bilinear;
        RippleMat = new Material(RippleShader);
        AddMat = new Material(AddShader);
        ScrollMat = new Material(ScrollShader);

        //Change the texture in the material of this object to the render texture calculated by the ripple shader.
        GetComponent<Renderer>().material.SetTexture("_RippleTex", CurrRT);
        //GetComponent<Renderer>().material.SetTexture("_MainTex", ObjectsRT);

        prevLocation = transform.position;
        offset = transform.position - targetTranform.position;
        StartCoroutine(ripples());
    }
    void Update() {
        transform.position = targetTranform.position + offset;
    }

    // Update is called once per frame
    IEnumerator ripples()
    {
        Vector3 playerOffset;
        while (true) {
            playerOffset = transform.position - prevLocation;
            if (playerOffset != Vector3.zero)
            {
                // Convert world offset to normalized UV offset
                Vector2 uvOffset = new Vector2(
                    playerOffset.x / rippleWorldSize,
                    playerOffset.z / rippleWorldSize
                );

                // Negative offset to keep ripples fixed in world space
                Vector4 offsetVec = new Vector4(-uvOffset.x, -uvOffset.y, 0, 0);
                ScrollMat.SetVector("_Offset", offsetVec);

                // Scroll CurrRT
                ScrollMat.SetTexture("_MainTex", CurrRT);
                Graphics.Blit(CurrRT, TempRT, ScrollMat);
                Graphics.Blit(TempRT, CurrRT);

                // Scroll PrevRT
                ScrollMat.SetTexture("_MainTex", PrevRT);
                Graphics.Blit(PrevRT, TempRT, ScrollMat);
                Graphics.Blit(TempRT, PrevRT);
            }


            //Copy the result of blending the render textures to TempRT.
            AddMat.SetTexture("_ObjectsRT", ObjectsRT);
            AddMat.SetTexture("_CurrentRT", CurrRT);
            Graphics.Blit(null, TempRT, AddMat);

            RenderTexture rt0 = TempRT;
            TempRT = CurrRT;
            CurrRT = rt0;
            //CurrRT holds the new blended one, 
            //TempRT holds original CurrRT, texture is reused in next step

            yield return new WaitForSeconds(0.00625f);

            //Calculate the ripple animation using ripple shader.
            RippleMat.SetTexture("_PrevRT", PrevRT);
            RippleMat.SetTexture("_CurrentRT", CurrRT);
            Graphics.Blit(null, TempRT, RippleMat);
            Graphics.Blit(TempRT, PrevRT);
            //PrevRT now holds the most recent ver

            //Swap PrevRT and CurrentRT to calculate the result for the next frame.
            RenderTexture rt = PrevRT;
            PrevRT = CurrRT;
            CurrRT = rt;

            prevLocation = transform.position;
            //Wait for two frames and then execute again.
            yield return new WaitForSeconds(0.018f);
        }
    }
}