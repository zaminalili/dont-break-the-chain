﻿namespace Application.DTOs;

public class ChainsRequestDto
{
    public Guid Id { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
