using Grpc.Core;
using Rira.PersonService.Application.Features.Persons;
using Rira.PersonService.Domain.Entities;

namespace Rira.PersonService.Grpc.Services;

public sealed class PersonGrpcService(
    CreatePerson.Handler create,
    GetPersonById.Handler get,
    ListPersons.Handler list,
    UpdatePerson.Handler update,
    DeletePerson.Handler delete) : PersonService.PersonServiceBase
{
    public override async Task<PersonReply> CreatePerson(CreatePersonRequest r, ServerCallContext c)
    {
        // چاپ مقدار دریافتی در کنسول برای دیباگ قطعی
        Console.WriteLine($"[DEBUG] Received BirthDate raw value: '{(r.BirthDate == null ? "NULL" : r.BirthDate)}'");

        if (string.IsNullOrWhiteSpace(r.BirthDate) || !DateOnly.TryParse(r.BirthDate.Trim(), out var date))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Invalid or missing birth_date. Raw received was: '{r.BirthDate}'"));
        }

        var x = await create.Handle(new CreatePerson.Command(r.FirstName, r.LastName, r.NationalCode, date), c.CancellationToken);
        return x.IsSuccess ? Map(x.Value!) : Errors.Throw<PersonReply>(x.Error);
    }


    public override async Task<PersonReply> GetPerson(GetPersonRequest r, ServerCallContext c)
    {
        if (!Guid.TryParse(r.Id, out var id))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "id must be a UUID."));

        var x = await get.Handle(new GetPersonById.Query(id), c.CancellationToken);
        return x.IsSuccess ? Map(x.Value!) : Errors.Throw<PersonReply>(x.Error);
    }

    public override async Task<ListPersonsReply> ListPersons(ListPersonsRequest r, ServerCallContext c)
    {
        var people = await list.Handle(new ListPersons.Query(r.Page, r.PageSize), c.CancellationToken);
        var result = new ListPersonsReply();
        result.Persons.Add(people.Select(Map));
        return result;
    }

    public override async Task<PersonReply> UpdatePerson(UpdatePersonRequest r, ServerCallContext c)
    {
        if (!Guid.TryParse(r.Id, out var id))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "id must be a UUID."));

        if (!DateOnly.TryParse(r.BirthDate?.Trim(), out var date))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "birth_date must be yyyy-MM-dd."));

        // اصلاح این خط: استفاده از UpdatePerson.Command به همراه id
        var x = await update.Handle(new UpdatePerson.Command(id, r.FirstName, r.LastName, r.NationalCode, date), c.CancellationToken);

        return x.IsSuccess ? Map(x.Value!) : Errors.Throw<PersonReply>(x.Error);
    }


    public override async Task<DeletePersonReply> DeletePerson(DeletePersonRequest r, ServerCallContext c)
    {
        if (!Guid.TryParse(r.Id, out var id))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "id must be a UUID."));

        var x = await delete.Handle(new DeletePerson.Command(id), c.CancellationToken);
        if (x.IsFailure) return Errors.Throw<DeletePersonReply>(x.Error);
        return new DeletePersonReply { Deleted = true };
    }

    private static PersonReply Map(Person p) => new()
    {
        Id = p.Id.ToString(),
        FirstName = p.FirstName,
        LastName = p.LastName,
        NationalCode = p.NationalCode,
        BirthDate = p.BirthDate.ToString("yyyy-MM-dd")
    };
}

internal static class Errors
{
    public static T Throw<T>(Rira.PersonService.Domain.Common.Errors.Error e)
    {
        var status = e.Code switch
        {
            "validation" => StatusCode.InvalidArgument,
            "not_found" => StatusCode.NotFound,
            "conflict" => StatusCode.AlreadyExists,
            _ => StatusCode.Internal
        };
        throw new RpcException(new Status(status, e.Description));
    }
}
