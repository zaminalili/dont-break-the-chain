namespace Domain.Exceptions;

public class NotFoundException(string resourceType, Guid resourceIdentifier) : Exception($"{resourceType} with id: {resourceIdentifier} not found.")
{
}
