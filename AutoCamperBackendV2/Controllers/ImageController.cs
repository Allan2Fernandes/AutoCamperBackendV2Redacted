using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoCamperBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        ParkInPeaceProjectContext context;
        public ImageController(ParkInPeaceProjectContext projectContext) 
        { 
            this.context = projectContext;
        }

        [HttpGet(nameof(getImageFileOnID) + "/{SpaceImageID}")]
        public async Task<ActionResult> getImageFileOnID(int SpaceImageID)
        {
            var TblSpaceImage = await context.TblSpaceImages.SingleOrDefaultAsync(EveryImage => EveryImage.FldSpaceImagesId == SpaceImageID);
            var fileDataAsBase64String = TblSpaceImage.FldB64encoding.Split(",")[1];


            // Decode the base64-encoded image data
            byte[] imageBytes = Convert.FromBase64String(fileDataAsBase64String);

            // Determine the appropriate content type based on the image data (e.g., "image/jpeg")
            string contentType = "";
            if (fileDataAsBase64String[0] == '/')
            {
                contentType = "image/jpeg";
            }
            else if (fileDataAsBase64String[0] == 'i')
            {
                contentType = "image/png";
            }

            // Return the image as a file with the appropriate content type and headers
            return File(imageBytes, contentType);
        }

        [AllowAnonymous]
        [HttpGet(nameof(GetImagesOfSpace) + "/{SpaceID}")]
        public async Task<ActionResult<List<object>>> GetImagesOfSpace(int SpaceID)
        {
            var query = from SpaceImages in context.TblSpaceImages
                        where SpaceImages.FldSpaceId == SpaceID
                        select SpaceImages.FldSpaceImagesId;

            List<object> Result = new List<object>();

            foreach (var item in query)
            {
                Result.Add(item);
            }
            return Ok(Result);
        }
    }



}
