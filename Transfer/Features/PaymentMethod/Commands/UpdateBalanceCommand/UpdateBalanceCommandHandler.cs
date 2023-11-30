using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transfer.Context;

namespace Transfer.Features.PaymentMethod.Commands.UpdateBalanceCommand;

public class UpdateBalanceCommandHandler : IRequestHandler<UpdateBalanceCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateBalanceCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        var pm = _context.PaymentMethods
            .Where(m => m.Id == request.Id)
            .FirstAsync(cancellationToken)
                 ?? throw new Exception("Счёт не найден!");

        pm.Result.Balance += request.Sum;

        await _context.SaveChangesAsync(cancellationToken);
        return await Task.FromResult(true);
    }
}