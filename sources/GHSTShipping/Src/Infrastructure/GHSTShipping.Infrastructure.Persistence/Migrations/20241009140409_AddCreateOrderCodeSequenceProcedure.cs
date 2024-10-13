using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCreateOrderCodeSequenceProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string createProcedureSql = @"
            IF OBJECT_ID('dbo.CreateOrderCodeSequenceAndRecord', 'P') IS NOT NULL
                BEGIN
                    DROP PROCEDURE dbo.CreateOrderCodeSequenceAndRecord;
                END
                GO

                CREATE PROCEDURE CreateOrderCodeSequenceAndRecord
                    @ShopId UNIQUEIDENTIFIER,
                    @ShopName VARCHAR(200),
                    @CreatedBy UNIQUEIDENTIFIER
                AS
                BEGIN
                    BEGIN TRY
                        BEGIN TRANSACTION;

                        DECLARE @SequenceName NVARCHAR(200) = CONVERT(NVARCHAR(128), @ShopName) + '_OrderCodeSequence';
                        DECLARE @SQL NVARCHAR(MAX);

                        -- Check if the sequence already exists and drop it if it does
                        SET @SQL = 'IF EXISTS (SELECT * FROM sys.sequences WHERE name = ' + QUOTENAME(@SequenceName, '''') + ')
                                    BEGIN
                                        DROP SEQUENCE ' + QUOTENAME(@SequenceName) + ';
                                    END';
                        EXEC sp_executesql @SQL;

                        -- Create the new sequence
                        SET @SQL = 'CREATE SEQUENCE ' + QUOTENAME(@SequenceName) + ' START WITH 1 INCREMENT BY 1;';
                        EXEC sp_executesql @SQL;

                        INSERT INTO ShopOrderCodeSequence (Id, SequenceName, CreatedBy, Created, IsDeleted, ShopId)
                        VALUES (NEWID(), @SequenceName, @CreatedBy, GETDATE(), 0, @ShopId);

                        COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;
                        THROW;
                    END CATCH
                END;
        ";

            migrationBuilder.Sql(createProcedureSql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string dropProcedureSql = @"
            DROP PROCEDURE IF EXISTS CreateOrderCodeSequenceAndRecord;
        ";

            migrationBuilder.Sql(dropProcedureSql);
        }
    }
}
