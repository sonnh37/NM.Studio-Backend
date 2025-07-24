// using AutoMapper;
// using NM.Studio.Domain.Contracts.Repositories;
// using NM.Studio.Domain.Contracts.UnitOfWorks;
// using NM.Studio.Domain.Models.Requests;
// using NM.Studio.Domain.Models.Responses;
// using NM.Studio.Domain.Models.Results.Bases;
// using NM.Studio.Domain.Utilities;
//
// namespace NM.Studio.Services;
//
// public class DashboardService
// {
//     protected readonly IMapper _mapper;
//     protected readonly IUnitOfWork _unitOfWork;
//     protected readonly IUserRepository _userRepository;
//     protected readonly IBookingRepository _bookingRepository;
//     protected readonly IAlbumRepository _albumRepository;
//     protected readonly IProductRepository _productRepository;
//     
//     protected DashboardService(IMapper mapper, IUnitOfWork unitOfWork)
//     {
//         _mapper = mapper;
//         _unitOfWork = unitOfWork;
//         _userRepository = _unitOfWork.UserRepository;
//         _bookingRepository = _unitOfWork.BookingRepository;
//         _albumRepository = _unitOfWork.AlbumRepository;
//         _productRepository = _unitOfWork.ProductRepository;
//     }
//     
//     public async Task<BusinessResult> GetStats(DashboardGetStatsQuery query)
//     {
//         try
//         {
//             var totalUsers = await _userRepository.GetTotalCount(query.FromDate, query.ToDate);
//             if (!results.Any())
//             {
//                 return new ResponseBuilder<TResult>()
//                     .WithData(results)
//                     .WithStatus(Const.NOT_FOUND_CODE)
//                     .WithMessage(Const.NOT_FOUND_MSG)
//                     .Build();
//             }
//
//             return new ResponseBuilder<TResult>()
//                 .WithData(results)
//                 .WithStatus(Const.SUCCESS_CODE)
//                 .WithMessage(Const.SUCCESS_READ_MSG)
//                 .Build();
//         }
//         catch (Exception ex)
//         {
//             string errorMessage = $"An error {typeof(TResult).Name}: {ex.Message}";
//             return new ResponseBuilder()
//                 .WithStatus(Const.FAIL_CODE)
//                 .WithMessage(errorMessage)
//                 .Build();
//         }
//     }
// }

