using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_SP_UpdateOrderCodeSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the SQL script to create the stored procedure
            migrationBuilder.Sql(@"
                IF OBJECT_ID('sp_UpdateOrderCodeSequence', 'P') IS NOT NULL
                BEGIN
                    DROP PROCEDURE sp_UpdateOrderCodeSequence;
                END
                GO
                CREATE PROCEDURE sp_UpdateOrderCodeSequence
                    @OrderIds NVARCHAR(MAX), -- Comma-separated list of Order IDs
                    @ShopId UNIQUEIDENTIFIER -- ShopId corresponding to the orders
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @NewSequence INT;
                    DECLARE @ShopUniqueCode NVARCHAR(50);
    
                    -- Bảng tạm để lưu kết quả cho từng đơn hàng
                    DECLARE @TempResults TABLE (
                        Id UNIQUEIDENTIFIER,
                        OrderCodeSequence NVARCHAR(50)
                    );

                    -- Table variable to hold the parsed and validated OrderIds
                    DECLARE @ParsedOrderIds TABLE (Id UNIQUEIDENTIFIER);
    
                    -- Split OrderIds into rows and safely convert to UNIQUEIDENTIFIER
                    INSERT INTO @ParsedOrderIds (Id)
                    SELECT TRY_CAST(value AS UNIQUEIDENTIFIER)
                    FROM STRING_SPLIT(@OrderIds, ',')
                    WHERE TRY_CAST(value AS UNIQUEIDENTIFIER) IS NOT NULL;

                    -- Ensure we have valid GUIDs
                    IF NOT EXISTS (SELECT 1 FROM @ParsedOrderIds)
                    BEGIN
                        -- If no valid OrderIds were found, return an error
                        RAISERROR('No valid Order IDs provided.', 16, 1);
                        RETURN;
                    END

                    -- Initialize @NewSequence
                    SET @NewSequence = 0;

	                -- Get ShopUniqueCode from the Shop table
                    SELECT @ShopUniqueCode = UniqueCode
                    FROM Shop
                    WHERE Id = @ShopId;

                    -- Check if there is data for the ShopId in CodeSequence
                    IF NOT EXISTS (SELECT 1 FROM CodeSequence WHERE ShopId = @ShopId)
                    BEGIN
                        -- If not exists, insert new record starting from 1
                        INSERT INTO CodeSequence (Id, ShopId, ShopUniqueCode, LastOrderCode, Created, CreatedBy, IsDeleted)
                        VALUES (NEWID(), @ShopId, @ShopUniqueCode, 1, GETDATE(), '00000000-0000-0000-0000-000000000000', 0);

                        -- Set @NewSequence to 1
                        SET @NewSequence = 1;
                    END
                    ELSE
                    BEGIN
                        -- If exists, update LastOrderCode and get the new value
                        UPDATE CodeSequence
                        SET LastOrderCode = LastOrderCode + 1,
                            LastModified = GETDATE()
                        WHERE ShopId = @ShopId;

                        -- Get the new LastOrderCode value
                        SELECT @NewSequence = LastOrderCode
                        FROM CodeSequence
                        WHERE ShopId = @ShopId;
                    END

                    -- Generate OrderCodeSequence for each order
                    DECLARE @OrderCodeSequence NVARCHAR(50);
    
                    INSERT INTO @TempResults (Id, OrderCodeSequence)
                    SELECT Id, @ShopUniqueCode + RIGHT('00000' + CAST(@NewSequence AS NVARCHAR(5)), 5)
                    FROM @ParsedOrderIds;

                    -- Update Order records with the newly generated OrderCodeSequence
                    UPDATE O
                    SET O.UniqueCode = TR.OrderCodeSequence, O.ClientOrderCode = TR.OrderCodeSequence
                    FROM [Order] O
                    INNER JOIN @TempResults TR ON O.Id = TR.Id;

                    -- Return the generated OrderCodeSequence and Id for each order
                    SELECT Id, OrderCodeSequence
                    FROM @TempResults;
                END;
                GO


            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the procedure if rolling back the migration
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_UpdateOrderCodeSequence;");
        }
    }
}
