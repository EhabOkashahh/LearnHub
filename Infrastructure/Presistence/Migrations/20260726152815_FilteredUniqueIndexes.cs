using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistence.Migrations
{
    /// <inheritdoc />
    public partial class FilteredUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP INDEX [UserNameIndex] ON [Users];
                CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
                    ON [Users]([NormalizedUserName])
                    WHERE [NormalizedUserName] IS NOT NULL AND [IsDeleted] = 0;
            ");

            migrationBuilder.Sql(@"
                DROP INDEX [EmailIndex] ON [Users];
                CREATE UNIQUE NONCLUSTERED INDEX [EmailIndex]
                    ON [Users]([NormalizedEmail])
                    WHERE [NormalizedEmail] IS NOT NULL AND [IsDeleted] = 0;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP INDEX [UserNameIndex] ON [Users];
                CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
                    ON [Users]([NormalizedUserName])
                    WHERE [NormalizedUserName] IS NOT NULL;
            ");

            migrationBuilder.Sql(@"
                DROP INDEX [EmailIndex] ON [Users];
                CREATE UNIQUE NONCLUSTERED INDEX [EmailIndex]
                    ON [Users]([NormalizedEmail])
                    WHERE [NormalizedEmail] IS NOT NULL;
            ");
        }
    }
}
