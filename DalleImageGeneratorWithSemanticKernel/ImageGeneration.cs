using DalleImageGeneratorWithSemanticKernel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.TextToImage;
using OpenAI.Images;
using System;
using System.Diagnostics;

namespace UseSemanticKernelFromNET
{
    public class ImageGeneration
    {
        public async Task GenerateBasicImage(string modelName)
        {
            Kernel kernel = Kernel
                .CreateBuilder()
                .AddOpenAITextToImage(modelId:modelName, apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY")!).Build();

            ITextToImageService imageService = kernel.GetRequiredService<ITextToImageService>();

            Console.WriteLine("##### SEMANTIC KERNEL - IMAGE GENERATOR DALL-E-3 CONSOLE APP #####\n\n");


            string prompt =
               """
                In the humorous image, Vice President JD Vance and his wife are seen stepping out of their plane onto the icy runway of
                Thule Air Base. Just as they set foot on the frozen ground, a bunch of playful polar bears greet them enthusiastically, much like 
                overzealous fans welcoming celebrities. The surprised expressions on their faces are priceless as the couple finds 
                themselves being "chased" by these bundles of fur and excitement. JD Vance, with a mix of amusement and alarm, has one 
                shoe comically left behind in the snow, while his wife, holding onto her hat against the chilly wind, can't suppress a laugh.
                The scene is completed with members of the Air Base
                staff in the background, chuckling and capturing the moment on their phones, adding to the light-heartedness of the unexpected encounter.  
                The plane should carry the AirForce One Colors and read "United States of America". 
             """;

            Console.WriteLine($"\n ### STORY FOR THE IMAGE TO GENERATE WITH DALL-E-3 ### \n{prompt}\n\n");

            Console.WriteLine("\n\nStarting generation of dall-e-3 image...");

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            var rotationTask = Task.Run(() => ConsoleUtil.RotateDash(cancellationToken), cts.Token);

            var image = await imageService.GetImageContentsAsync(prompt, new OpenAITextToImageExecutionSettings {                 
                Size = (1024, 1024), 
                Style = "vivid",
                Quality = "hd", //high
                ResponseFormat = "b64_json" // bytes
            });

            cts.Cancel(); //cancel to stop animating the waiting indicator

            var imageTmpFilePng = Path.ChangeExtension(Path.GetTempFileName(), "png");
            image?.FirstOrDefault()?.WriteToFile(imageTmpFilePng);

            Console.WriteLine($"Wrote image to location: {imageTmpFilePng}");

            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = imageTmpFilePng,
                UseShellExecute = true
            });

        }

    }
}
 