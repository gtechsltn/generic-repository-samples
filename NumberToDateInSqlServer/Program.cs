﻿using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using NumberToDateInSqlServer.Models;
using kp.Dapper.Handlers;

namespace NumberToDateInSqlServer;

internal partial class Program
{
    static void Main(string[] args)
    {
        Operations operations = new Operations();
        List<ChallengeTable> data = operations.GetAll();
        Console.WriteLine(ObjectDumper.Dump(data));
        Console.ReadLine();
    }
}

/// <summary>
/// Belongs in its own file but for this it is easier to work with.
/// </summary>
internal class Operations
{
    private IDbConnection _cn;

    public Operations()
    {
        SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());
        _cn = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Examples;Integrated Security=True;Encrypt=False");
    }
    /*
     * Both statements work, it's a matter of preference 
     */
    public List<ChallengeTable> GetAll()
    {
        // Karen @KarenPayneMVP
        var statement1 = 
            """
            SELECT Id, 
                   DateValue,
                   CAST(CAST(DateValue AS CHAR(8)) AS DATE) AS DateItem 
            FROM Examples.dbo.ChallengeTable;
            """;

        // João Silva @kappyzor
        var statement2 =
            """
            SELECT Id,
                   DateValue, 
                   DATEFROMPARTS(DateValue / 10000, (DateValue - (DateValue / 10000) * 10000) / 100, (DateValue - (DateValue / 100) * 100)) DateItem 
            FROM Examples.dbo.ChallengeTable;
            """;

        return _cn.Query<ChallengeTable>(
            """
            Pick one of the statements above, both work
            """).ToList();
    }
}