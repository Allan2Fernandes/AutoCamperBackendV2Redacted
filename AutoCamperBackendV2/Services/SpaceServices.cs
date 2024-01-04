using AutoCamperBackendV2.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace AutoCamperBackendV2.Services
{
    public class SpaceServices
    {
        ParkInPeaceProjectContext context;

        public SpaceServices(ParkInPeaceProjectContext context)
        {
            this.context = context;
        }

        public TblSpace CreateSpaceAdvertisement(CreateTblSpaceDTO CreateTblSpaceDTO)
        {
            // Add the space to the database
            TblSpace NewTblSpaceEntry = new TblSpace
            {
                FldSpaceId = CreateTblSpaceDTO.FldSpaceId,
                FldPrice = CreateTblSpaceDTO.FldPrice,
                FldAddress = CreateTblSpaceDTO.FldAddress,
                FldUserId = CreateTblSpaceDTO.FldUserId,
                FldLength = CreateTblSpaceDTO.FldLength,
                FldWidth = CreateTblSpaceDTO.FldWidth,
                FldHeight = CreateTblSpaceDTO.FldHeight,
                FldLongitude = CreateTblSpaceDTO.FldLongitude,
                FldLatitude = CreateTblSpaceDTO.FldLatitude,
                FldSewageDisposal = CreateTblSpaceDTO.FldSewageDisposal,
                FldElectricity = CreateTblSpaceDTO.FldElectricity,
                FldCancellationDuration = CreateTblSpaceDTO.FldCancellationDuration,
                FldCancellationPenalty = CreateTblSpaceDTO.FldCancellationPenalty,
                FldIsActive = true
            };

            context.TblSpaces.Add(NewTblSpaceEntry);
            context.SaveChanges();
            return NewTblSpaceEntry;
        }

        public void AddSpaceImages(List<TblSpaceImage> spaceImages)
        {
            context.TblSpaceImages.AddRange(spaceImages);
            context.SaveChanges();
        }

        public IQueryable<object> GetSpaceAdvertisementsInArea(GetAdvertisementsInAreaDTO RequestBody, double CoordinatesRadius)
        {
            double long_center = (double)RequestBody.FldLongitude;
            double lat_center = (double)RequestBody.FldLatitude;

            double lat_min = lat_center - CoordinatesRadius;
            double lat_max = lat_center + CoordinatesRadius;
            double long_min = long_center - (CoordinatesRadius / Math.Cos(lat_center * Math.PI / 180));
            double long_max = long_center + (CoordinatesRadius / Math.Cos(lat_center * Math.PI / 180));

            // Using the coordinates range, find all the advertisements in that range
            //List<TblSpace> QueriedResults = await context.TblSpaces.Where((space) => space.FldLongtitude > long_min && space.FldLongtitude < long_max && space.FldLatitude > lat_min && space.FldLatitude < lat_max).Include(space => space.).ToListAsync();
            var query = from tblSpaces in context.TblSpaces
                        join tblSpaceImages in context.TblSpaceImages on tblSpaces.FldSpaceId equals tblSpaceImages.FldSpaceId
                        join tblBookings in context.TblBookings on tblSpaces.FldSpaceId equals tblBookings.FldSpaceId into BookingGroup
                        from booking in BookingGroup.DefaultIfEmpty()
                        where tblSpaces.FldLongitude > long_min
                            && tblSpaces.FldLongitude < long_max
                            && tblSpaces.FldLatitude > lat_min
                            && tblSpaces.FldLatitude < lat_max
                            // Filter out the prices if a threshold is specified
                            && (RequestBody.FldPrice <= 0 || tblSpaces.FldPrice < RequestBody.FldPrice)
                            && tblSpaces.FldIsActive == true
                        select new
                        {
                            tblSpaces, // Include all properties from TblSpaces
                            BookingData = booking,
                            tblSpaceImages.FldSpaceImagesId // Include FldSpaceImagesId from TblSpaceImages
                        };


            var GroupedResults = query.GroupBy(x => x.tblSpaces.FldSpaceId).Select(group => new
            {
                TblSpace = group.First().tblSpaces, // Include the first TblSpaces entry in the group
                FldSpaceImagesIds = group.Select(item => item.FldSpaceImagesId).Distinct().ToList(),
                Bookings = group.Select(item => item.BookingData).Distinct().ToList()
            });

            return GroupedResults;
        }

        public IQueryable<object> GetSpaceDetailsOnSpaceID(int SpaceID)
        {
            var query = from tblSpaces in context.TblSpaces
                        join tblSpaceImages in context.TblSpaceImages on tblSpaces.FldSpaceId equals tblSpaceImages.FldSpaceId
                        join tblBookings in context.TblBookings on tblSpaces.FldSpaceId equals tblBookings.FldSpaceId into BookingGroup
                        from booking in BookingGroup.DefaultIfEmpty()
                        where tblSpaces.FldSpaceId == SpaceID
                        select new
                        {
                            tblSpaces, // Include all properties from TblSpaces
                            BookingData = booking,
                            tblSpaceImages.FldSpaceImagesId // Include FldSpaceImagesId from TblSpaceImages
                        };


            var GroupedResults = query.GroupBy(x => x.tblSpaces.FldSpaceId).Select(group => new
            {
                TblSpace = group.First().tblSpaces, // Include the first TblSpaces entry in the group
                FldSpaceImagesIds = group.Select(item => item.FldSpaceImagesId).Distinct().ToList(),
                Bookings = group.Select(item => item.BookingData).Distinct().ToList()
            }); 
            return GroupedResults;
        }

        public List<object> GetSpacesOnOwnerUserID(int OwnerID)
        {
            var query = from tblSpaces in context.TblSpaces
                        where tblSpaces.FldUserId == OwnerID && tblSpaces.FldIsActive == true
                        select tblSpaces;

            List<object> QueriedList = new List<object>();
            foreach (var item in query)
            {
                QueriedList.Add(new
                {
                    SpaceID = item.FldSpaceId,
                    Price = item.FldPrice,
                    Address = item.FldAddress,
                    OnwerID = item.FldUserId,
                    Length = item.FldLength,
                    Width = item.FldWidth,
                    Height = item.FldHeight,
                    Longitude = item.FldLongitude,
                    Latitude = item.FldLatitude,
                    SewageDisposal = item.FldSewageDisposal,
                    Electricity = item.FldElectricity,
                });
            }
            return QueriedList;
        }

        public TblSpace? GetSpaceOnSpaceID(int SpaceID)
        {
            var Space = context.TblSpaces.SingleOrDefault(eachSpace => eachSpace.FldSpaceId == SpaceID);
            return Space;
        }

        public void UpdateSpaceDetails(TblSpace? SpaceToUpdate, UpdateSpaceDTO RequestBody)
        {
            SpaceToUpdate.FldPrice = RequestBody.FldPrice;
            SpaceToUpdate.FldAddress = RequestBody.FldAddress;
            SpaceToUpdate.FldLength = RequestBody.FldLength;
            SpaceToUpdate.FldWidth = RequestBody.FldWidth;
            SpaceToUpdate.FldHeight = RequestBody.FldHeight;
            SpaceToUpdate.FldLongitude = RequestBody.FldLongitude;
            SpaceToUpdate.FldLatitude = RequestBody.FldLatitude;
            SpaceToUpdate.FldSewageDisposal = RequestBody.FldSewageDisposal;
            SpaceToUpdate.FldElectricity = RequestBody.FldElectricity;

            context.SaveChanges();
        }

        public List<TblBooking>? GetOngoingAndFutureBookings(DeactivateSpaceDTO RequestBody)
        {
            var OngoingBookingsForSpace = context.TblBookings.Where(EachBooking => EachBooking.FldSpaceId == RequestBody.FldSpaceId && EachBooking.FldReservationEnd > DateTime.Now).ToList();
            return OngoingBookingsForSpace;
        }

        public void DeactivateSpace(TblSpace SpaceToUpdate)
        {
            SpaceToUpdate.FldIsActive = false;
            context.SaveChanges();
        }
    }
}
