using NatML;
using NatML.Features;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Diagnostics;

class ModelPredictor : IMLPredictor<float[]>
{

    public static async Task<ModelPredictor> CreateFromFile(MLModelData data)
    {
        // Load edge model
        var model = await MLEdgeModel.Create(data);
        // Create predictor
        var predictor = new ModelPredictor(model);
        // Return predictor
        return predictor;
    }

    private MLEdgeModel model;

    private ModelPredictor(MLEdgeModel model)
    {
        this.model = model;
        //  TO print the details of the model being used
        UnityEngine.Debug.Log(model);
    }

    public float[] Predict(params MLFeature[] inputs)
    {
        // Check that the input is an flatline feature
       if (!(inputs[0] is MLArrayFeature<float> arrayFeature))
            throw new ArgumentException(@"Predictor makes predictions on array features");

        MLFeatureType inputType = model.inputs[0];

        using MLEdgeFeature edgeFeature = (inputs[0] as IMLEdgeFeature).Create(inputType);
        using var outputFeatures = model.Predict(edgeFeature);
        var result = new MLArrayFeature<float>(outputFeatures[0]);
       
        return result.ToArray(); 
    }
    //return new float[1];
    //  Debug.Assert( inputType is MLArrayType);

    // Debug.Assert(edgeFeature.shape == ((MLArrayType) inputType).shape);
    public void Dispose()
    {
    }

}
