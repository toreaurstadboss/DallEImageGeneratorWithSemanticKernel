using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Services;
using Microsoft.SemanticKernel.TextToImage;

namespace UseSemanticKernelFromNET;

public static class TextToImageServiceExtensions
{


    /// <summary>
    /// Generates OpenAI image content asynchronously based on the provided text input and settings.
    /// </summary>
    /// <param name="imageService">The image service used to generate the image content.</param>
    /// <param name="input">The text input used to generate the image.</param>
    /// <param name="kernel">An optional kernel instance for additional processing.</param>
    /// <param name="size">
    /// The desired size of the generated image. For DALL-E 2 images, use: 256x256, 512x512, or 1024x1024. 
    /// For DALL-E 3 images, use: 1024x1024, 1792x1024, or 1024x1792.
    /// </param>
    /// <param name="style">The style of the image. Must be "vivid" or "natural".</param>
    /// <param name="quality">The quality of the image. Must be "standard", "hd", or "high".</param>
    /// <param name="responseFormat">
    /// The format of the response. Must be one of the following: "url", "uri", "b64_json", or "bytes".
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a read-only list of 
    /// <see cref="ImageContent"/> objects representing the generated images.
    /// </returns>
    public static Task<IReadOnlyList<ImageContent>> GetOpenAIImageContentAsync(this ITextToImageService imageService,
        TextContent input,
        Kernel? kernel = null,
        (int width, int height) size = default((int, int)), // for Dall-e-2 images, use: 256x256, 512x512, or 1024x1024. For dalle-3 images, use: 1024x1024, 1792x1024, 1024x1792. 
        string style = "vivid",
        string quality = "hd",
        string responseFormat = "b64_json",        
        CancellationToken cancellationToken = default)
    {
        
        string? currentModelId = imageService.GetModelId();

        if (currentModelId != "dall-e-3" && currentModelId != "dall-e-2")
        {
            throw new NotSupportedException("This method is only supported for the DALL-E 2 and DALL-E 3 models.");
        }

        if (size.width == 0 || size.height == 0)
        {
            size = (1024, 1024); //defaulting here to (1024, 1024).
        }

        if (currentModelId == "dall-e-2"){
            var supportedSizes = new[]{
                (256, 256),
                (512, 512),
                (1024, 1024)
            };
            if (!supportedSizes.Contains(size))
            {
                throw new ArgumentException("For DALL-E 2, the size must be one of: 256x256, 512x512, or 1024x1024.");
            }
        }
        else if (currentModelId == "dall-e-3")
        {
            var supportedSizes = new[]{
                (1024, 1024),
                (1792, 1024),
                (1024, 1792)
            };
            if (!supportedSizes.Contains(size))
            {
                throw new ArgumentException("For DALL-E 3, the size must be one of: 1024x1024, 1792x1024 or 1024x1792.");
            }
        }

        return imageService.GetImageContentsAsync(
            input,
            new OpenAITextToImageExecutionSettings
                {
                    Size = size,
                    Style = style, //must be "vivid" or "natural"
                    Quality = quality, //must be "standard" or "hd" or "high"
                    ResponseFormat = responseFormat // url or uri or b64_json or bytes
                },
            kernel,
            cancellationToken);

    }
}
