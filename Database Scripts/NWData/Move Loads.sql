USE [NW_DataDev2]
GO

/****** Object:  StoredProcedure [dbo].[Change_Harvest_Load_Weight_Sheet]    Script Date: 1/2/2025 4:25:27 PM ******/
DROP PROCEDURE [dbo].[Change_Harvest_Load_Weight_Sheet]
GO

/****** Object:  StoredProcedure [dbo].[Change_Harvest_Load_Weight_Sheet]    Script Date: 1/2/2025 4:25:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Change_Harvest_Load_Weight_Sheet]
(
	@Load_UID uniqueidentifier,
	@Weight_Sheet_UID uniqueidentifier,
	@Reason nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT ON;
	
		declare @Original_Weight_Sheet_Id BIGINT,
		@Original_Weight_Sheet_UID UNIQUEIDENTIFIER,
		@New_Weight_Sheet_Id BIGINT,
		@Load_Id BIGINT,
		@LoadUID UNIQUEIDENTIFIER,
		@SourceID BIGINT,
		@Protein REAL,
		@Comment NVARCHAR(500),
		@Transfer_Load_UID UNIQUEIDENTIFIER,
		@Weight_Sheet_Transfer_Load_UID UNIQUEIDENTIFIER,
		@Transfer_Load_Load_Id BIGINT,
		
		@Source_Bin nvarchar(50);

	
			BEGIN TRANSACTION;
	BEGIN TRY

		SELECT @Load_Id = loads.Load_Id 
		FROM loads 
		WHERE UID = @Load_UID;
		print @Load_UID

		SELECT @Original_Weight_Sheet_Id = Weight_Sheets.WS_Id,
			   @Original_Weight_Sheet_UID = Weight_Sheets.UID
		FROM Weight_Sheets
		INNER JOIN Inbound_Loads ON Weight_Sheets.UID = Inbound_Loads.WS_UID
		INNER JOIN Loads ON Inbound_Loads.Load_UID = Loads.UID
		WHERE Inbound_Loads.Load_UID = @Load_UID;
		--Cannot Find Inbound Load So Find the Transfer Load
		
		IF Not Exists (SELECT * FROM Inbound_Loads WHERE (Load_UID = @Load_UID))
		BEGIN

			select @Weight_Sheet_Transfer_Load_UID = Weight_Sheet_Transfer_Loads.UID from Weight_Sheet_Transfer_Loads where ( Weight_Sheet_Transfer_Loads.Weight_Sheet_UID= @Weight_Sheet_UID)
		
			SELECT @Transfer_Load_UID = Transfer_Loads.UID,
				   @Source_Bin= Transfer_Loads.Source_Bin,
				   @Protein= Transfer_Loads.Protein
			FROM Transfer_Loads
			WHERE Load_UID = @Load_UID;
			   PRINT 'Transfer_Load_UID: ' + CAST(@Transfer_Load_UID AS NVARCHAR(36)) + 
			   ', Weight_Sheet_UID: ' + CAST(@Weight_Sheet_UID AS NVARCHAR(36)) + 
			   ', Protein: ' + CAST(isnull(@Protein,0) AS NVARCHAR(50));

			insert into Transfer_Loads (Load_UID, Transfer_Load_UID,Protein)
			values (@Load_UID, @Weight_Sheet_Transfer_Load_UID, @Protein);
			delete from Inbound_Loads where Load_UID = @Load_UID;
		END
		else
		BEGIN
		 -- it is an inbound load
		 
		--See if the new Weight Sheet is a transfer
		if exists(SELECT *  FROM Weight_Sheet_Transfer_Loads WHERE (Weight_Sheet_UID = @Weight_Sheet_UID))
		begin
		 SELECT  @Protein = Protien FROM Inbound_Loads WHERE (Load_UID = @Load_UID)
		 SELECT @Weight_Sheet_Transfer_Load_UID=UID
		        FROM Weight_Sheet_Transfer_Loads
				WHERE (Weight_Sheet_UID = @Weight_Sheet_UID);
				if not exists (select * from Transfer_Loads where (Load_UID =@Load_UID))
				begin

				 INSERT INTO Transfer_Loads
                         (Load_UID, Protein, Transfer_Load_UID)
				VALUES        (@Load_UID,@Protein,@Weight_Sheet_Transfer_Load_UID);
				
				end
		 DELETE FROM Inbound_Loads WHERE (Load_UID = @Load_Uid)
		end
		else
		begin
		print 'Updated'
			UPDATE Inbound_Loads SET WS_UID = @Weight_Sheet_UID WHERE Load_UID = @Load_UID;
		end
		end

		SET @Comment = 'Moved Ticket:' + CONVERT(NVARCHAR(15), @Load_Id) + 
					   ' From Weight Sheet:' + CONVERT(NVARCHAR(15), @Original_Weight_Sheet_Id) + 
					   ' to Weight Sheet:' + CONVERT(NVARCHAR(15), @New_Weight_Sheet_Id) + 
					   ' Reason :' + @Reason;



		UPDATE Weight_Sheets SET Comment = Comment + @Comment WHERE UID = @Original_Weight_Sheet_UID OR UID = @Weight_Sheet_UID;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END
GO


/****** Object:  StoredProcedure [dbo].[Change_Transfer_Load_Weight_Sheet]    Script Date: 1/2/2025 4:25:37 PM ******/
DROP PROCEDURE [dbo].[Change_Transfer_Load_Weight_Sheet]
GO

/****** Object:  StoredProcedure [dbo].[Change_Transfer_Load_Weight_Sheet]    Script Date: 1/2/2025 4:25:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Change_Transfer_Load_Weight_Sheet]
(@Load_UID uniqueidentifier,
@Weight_Sheet_UID uniqueidentifier,
@Reason nvarchar(255)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @Transfer_Loads_UID uniqueidentifier,
	 @Weight_Sheet_Transfer_Loads_UID uniqueidentifier,
	 @Original_Weight_Sheet_Id bigint,
	 @Original_Weight_Sheet_UID uniqueidentifier,
	 @New_Weight_Sheet_Id bigint,
	 @Load_Id bigint,
	 @Comment nvarchar(500),
	 @Protein real,
	 @InboundLoadUID uniqueidentifier




	BEGIN TRANSACTION;
	
	BEGIN TRY
-- Get the Weight Sheet Transfer Load UID For The Weight Sheet to move to    
SELECT       @Weight_Sheet_Transfer_Loads_UID=   UID
FROM            Weight_Sheet_Transfer_Loads
WHERE        (Weight_Sheet_UID = @Weight_Sheet_UID)

--Cannot Find the Weight Sheet Transfer So we are moving it to an intake load

IF @Weight_Sheet_Transfer_Loads_UID IS NULL
BEGIN
  SELECT  @Protein= Transfer_Loads.Protein   from Transfer_Loads where (Load_UID=@Load_UID);
	
  SELECT    @Original_Weight_Sheet_Id=    Weight_Sheets.WS_Id
		FROM            Weight_Sheets INNER JOIN
                         Weight_Sheet_Transfer_Loads ON Weight_Sheets.UID = Weight_Sheet_Transfer_Loads.Weight_Sheet_UID INNER JOIN
                         Transfer_Loads ON Weight_Sheet_Transfer_Loads.UID = Transfer_Loads.Transfer_Load_UID

  select @InboundLoadUID= UID  from Inbound_Loads Where (Inbound_Loads.WS_UID= @Weight_Sheet_UID and Inbound_Loads.Load_UID= @Load_UID)
  if @InboundLoadUID is null INSERT INTO Inbound_Loads (Load_UID, WS_UID,Protien) values (@Load_UID,@Weight_Sheet_UID,@Protein)
  DELETE FROM Transfer_Loads WHERE( Load_UID=@Load_UID)

END
ELSE
BEGIN

	-- Get the Transfer Load UID using Weight Sheet UID that will be moving
	SELECT       @Transfer_Loads_UID= Transfer_Loads.UID 
	FROM            Weight_Sheet_Transfer_Loads INNER JOIN
							 Transfer_Loads ON Weight_Sheet_Transfer_Loads.UID = Transfer_Loads.Transfer_Load_UID INNER JOIN
							 Loads ON Transfer_Loads.Load_UID = Loads.UID
	WHERE        (Loads.UID = @Load_UID)

	SELECT       @Load_Id= Load_Id,@Original_Weight_Sheet_UID=Weight_Sheet_Transfer_Loads.Weight_Sheet_UID,@Original_Weight_Sheet_Id= Weight_Sheets.WS_Id
	FROM            Loads INNER JOIN
							 Transfer_Loads ON Loads.UID = Transfer_Loads.Load_UID INNER JOIN
							 Weight_Sheet_Transfer_Loads ON Transfer_Loads.Transfer_Load_UID = Weight_Sheet_Transfer_Loads.UID INNER JOIN
							 Weight_Sheets ON Weight_Sheet_Transfer_Loads.Weight_Sheet_UID = Weight_Sheets.UID
	WHERE        (Loads.UID = @Load_UID)
	update Transfer_Loads set Transfer_Load_UID=@Weight_Sheet_Transfer_Loads_UID
	WHERE        (Load_UID  = @Load_UID)
END





-- Get Stuff For The Comments
set @Comment=' Moved Ticket:'+convert(nvarchar(15),@Load_Id )+ ' From Weight Sheet:'+convert(nvarchar(15),@Original_Weight_Sheet_Id)+' to Weight Sheet:'+ convert(nvarchar(15),@New_Weight_Sheet_Id)+' Reason :' +@Reason
-- print '@Weight_Sheet_UID   '+ Convert(nvarchar(50),@Weight_Sheet_UID)
--print '@Weight_Sheet_Transfer_Loads_UID   '+ Convert(nvarchar(50),@Weight_Sheet_Transfer_Loads_UID)
--print '@Transfer_Loads_UID   '+ Convert(nvarchar(50), @Transfer_Loads_UID)
--print '@Load_UID   '+ Convert(nvarchar(50), @Load_UID)


	update Weight_Sheets set Comment=Comment +@Comment where UID=@Original_Weight_Sheet_UID or UID = @Weight_Sheet_UID


			COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END

GO



