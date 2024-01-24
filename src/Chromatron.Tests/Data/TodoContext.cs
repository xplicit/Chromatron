// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Tests.Data;

public class TodoContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseInMemoryDatabase(databaseName: "ChromatronTest_" + DateTime.Now.ToString("yyyyMMdd"));

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public DbSet<TodoItem> TodoItems { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}