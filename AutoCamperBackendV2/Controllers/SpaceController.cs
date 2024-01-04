using AutoCamperBackendV2.DataTransferObjects;
using AutoCamperBackendV2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AutoCamperBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SpaceController : ControllerBase
    {
        private readonly ParkInPeaceProjectContext context;
        private SpaceServices Services;
        double CoordinatesRadius;

        public SpaceController(ParkInPeaceProjectContext context)
        {
            this.context = context;
            this.Services = new SpaceServices(context);
            CoordinatesRadius = 0.045;
        }

        [HttpPost(nameof(CreateSpaceAdvertisement))]
        public async Task<ActionResult<string>> CreateSpaceAdvertisement(CreateTblSpaceDTO CreateTblSpaceDTO)
        {
            TblSpace NewTblSpaceEntry = Services.CreateSpaceAdvertisement(CreateTblSpaceDTO);            

            // using the ID, save the iamges
            int SetSpaceID = NewTblSpaceEntry.FldSpaceId;
            List<TblSpaceImage> spaceImages = new List<TblSpaceImage>();
            foreach (string ImageEncoding in CreateTblSpaceDTO.ListOfImageEncodings)
            {
                TblSpaceImage image = new TblSpaceImage
                {
                    FldB64encoding = ImageEncoding,
                    FldSpaceId = SetSpaceID
                };
                spaceImages.Add(image);
            }
            Services.AddSpaceImages(spaceImages);
            return Ok("Space Saved");
        }

        [AllowAnonymous]
        [HttpPost(nameof(GetAdvertisementsInArea))]
        public async Task<ActionResult<IQueryable<object>>> GetAdvertisementsInArea(GetAdvertisementsInAreaDTO RequestBody)
        {
            var GroupedResults = Services.GetSpaceAdvertisementsInArea(RequestBody, CoordinatesRadius);             
            return Ok(GroupedResults);
        }

        [AllowAnonymous]
        [HttpGet(nameof(GetSpaceDetailsOnSpaceID) + "/{SpaceID}")]
        public async Task<ActionResult<IQueryable<object>>> GetSpaceDetailsOnSpaceID(int SpaceID)
        {
            // Using the coordinates range, find all the advertisements in that range
            var GroupedResults = Services.GetSpaceDetailsOnSpaceID(SpaceID);           

            return Ok(GroupedResults);
        }

        [HttpGet(nameof(getBookingsOfSpaceOnSpaceID) + "/{SpaceID}")] // Move this to the bookings controller
        public async Task<ActionResult<List<object>>> getBookingsOfSpaceOnSpaceID(int SpaceID)
        {            
            return Ok("Not Implemented");
        }

        [HttpGet(nameof(getSpacesOnOwnerUserID) + "/{OwnerID}")]
        public async Task<ActionResult<List<object>>> getSpacesOnOwnerUserID(int OwnerID)
        {
          
            List<object> QueriedList = Services.GetSpacesOnOwnerUserID(OwnerID);            
            return Ok(QueriedList);
        }
        // TODO MANUALLY TEST THIS ONE
        [HttpPut(nameof(UpdateSpaceDetails))]
        public async Task<ActionResult<string>> UpdateSpaceDetails(UpdateSpaceDTO RequestBody)
        {
            // Find the Space to update
            var SpaceToUpdate = Services.GetSpaceOnSpaceID(RequestBody.FldSpaceId);
            if (SpaceToUpdate == null)
            {
                return Ok(new {Result = "Space Not Found"});
            }

            Services.UpdateSpaceDetails(SpaceToUpdate, RequestBody);

            return Ok(new {Result = "Space Updated"});
        }

        // TODO MANUALLY TEST THIS ONE
        [HttpDelete(nameof(DeactivateSpace))]
        public async Task<ActionResult<string>> DeactivateSpace(DeactivateSpaceDTO RequestBody)
        {
            // If a space has active bookings in the future, do not deactivate the space

            // Find the space
            //var SpaceToUpdate = await context.TblSpaces.SingleOrDefaultAsync(EachSpace => EachSpace.FldSpaceId == RequestBody.FldSpaceId);
            var SpaceToUpdate = Services.GetSpaceOnSpaceID(RequestBody.FldSpaceId);

            // Find bookings with the reservation end date after date.now
            var OngoingBookingsForSpace = Services.GetOngoingAndFutureBookings(RequestBody);

            if(OngoingBookingsForSpace.Count > 0)
            {
                return Ok("Ongoing Bookings");
            }
            else
            {
                Services.DeactivateSpace(SpaceToUpdate);
                return Ok("Cancelled");
            }
        }

       

    }
}
