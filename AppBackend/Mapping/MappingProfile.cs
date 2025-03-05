using AutoMapper;
using AppBackend.Models;
using AppBackend.DTOs;

namespace AppBackend.Mapping{


    public class MappingProfile: Profile{
        public MappingProfile(){
            CreateMap<Deltas,DeltasDto>();
            CreateMap<SouthWests,SouthwestsDto>();
            CreateMap<CombineFlight,CombinedFlightDto>();
            CreateMap<ConnectingFlight,ConnectingFlightDto>();
        }
    }
}