using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatML;
using NatML.Features;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

public class PredictDataScript : MonoBehaviour
{
    // the shape of the input data to the predictor
    int[] INPUT_SHAPE = { 1, 512, 6 };

    public MLModelData model_android;
    public MLModelData model_windows;
    public HeadsetMotionGetter motion_getter;

    public float[] send_output;

    private ModelPredictor predictor;

    private MLArrayFeature<float> databuffer;

    // Start is called before the first frame update
    void Start()
    {
        databuffer = new MLArrayFeature<float>(new float[INPUT_SHAPE[0] * INPUT_SHAPE[1] * INPUT_SHAPE[2]], INPUT_SHAPE);
        CreateModel();
    }

    // Function for creating model
    async void CreateModel()
    {
        if (UnityEngine.Application.isEditor)
         {
             predictor = await ModelPredictor.CreateFromFile(model_windows);
         }
         else
         {
            predictor = await ModelPredictor.CreateFromFile(model_android);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (predictor == null) return;

        int datalen = databuffer.shape[1];
        // shift the history in the buffer along
        for (int c = datalen - 1; c > 0; c--)
        {
            databuffer[0, c, 0] = databuffer[0, c - 1, 0];
        }
        //'CEP_x','CEP_z','CEP_y','CEV_z','CEV_x','CEA_z','CER_y','CEAV_x','CER_w'

          Vector3 posi = motion_getter.outputs["<XRHMD>/centerEyePosition"];
          databuffer[0, 0, 0] = posi.x;
          databuffer[0, 0, 1] = posi.y;
          databuffer[0, 0, 2] = posi.z;

         /* Vector3 velo = motion_getter.outputs["<XRHMD>/centerEyeVelocity"];
          databuffer[0, 0, 3] = velo.x;
          databuffer[0, 0, 4] = velo.y;
          databuffer[0, 0, 5] = velo.z; 

          Vector3 acl = motion_getter.outputs["<XRHMD>/centerEyeAcceleration"];
          databuffer[0, 0, 6] = acl.x;
          databuffer[0, 0, 7] = acl.y;
          databuffer[0, 0, 8] = acl.z;

          Vector4 rotn = motion_getter.outputs["<XRHMD>/centerEyeRotation"];
          databuffer[0, 0, 9] = rotn.x;
          databuffer[0, 0, 10] = rotn.y;
          databuffer[0, 0, 11] = rotn.z;
          databuffer[0, 0, 12] = rotn.w; */

          Vector3 angvel = motion_getter.outputs["<XRHMD>/centerEyeAngularVelocity"];
          databuffer[0, 0, 3] = angvel.x;
          databuffer[0, 0, 4] = angvel.y;
          databuffer[0, 0, 5] = angvel.z;



        float[] outputs = predictor.Predict(databuffer);
        //UnityEngine.Debug.Log("Output data: " + outputs[0]);
        send_output = outputs;
        
    }
}
