using AutoMapper;
using WMTemplate.Application.Common.Interfaces;
using WMTemplate.Domain.Repositories.Base;

namespace WMTemplate.Application.Services;

public abstract class BaseService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser)
{
    protected readonly IUnitOfWork UnitOfWork = unitOfWork;
    protected readonly IMapper Mapper = mapper;
    protected readonly ICurrentUser CurrentUser = currentUser;
}
