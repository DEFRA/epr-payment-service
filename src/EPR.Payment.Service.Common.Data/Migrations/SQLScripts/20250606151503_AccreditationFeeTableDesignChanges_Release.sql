BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Lookup].[AccreditationFees]') AND [c].[name] = N'TonnesOver');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Lookup].[AccreditationFees] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Lookup].[AccreditationFees] DROP COLUMN [TonnesOver];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Lookup].[AccreditationFees]') AND [c].[name] = N'TonnesUpTo');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Lookup].[AccreditationFees] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Lookup].[AccreditationFees] DROP COLUMN [TonnesUpTo];
GO

ALTER TABLE [Lookup].[AccreditationFees] ADD [TonnageBandId] int NOT NULL DEFAULT 1;
GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 3;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 4;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 5;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 6;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 7;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 8;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 9;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 10;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 11;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 12;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 13;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 14;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 15;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 16;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 17;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 18;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 19;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 20;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 21;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 22;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 23;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 24;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 25;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 26;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 27;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 28;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 29;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 30;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 31;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 32;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 33;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 34;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 35;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 36;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 37;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 38;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 39;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 40;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 41;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 42;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 43;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 44;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 45;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 46;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 47;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 48;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 49;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 50;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 51;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 52;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 53;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 54;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 55;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 56;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 57;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 58;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 59;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 60;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 61;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 62;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 63;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 64;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 65;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 66;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 67;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 68;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 69;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 70;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 71;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 72;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 73;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 74;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 75;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 76;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 77;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 78;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 79;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 80;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 81;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 82;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 83;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 84;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 85;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 86;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 87;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 88;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 89;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 90;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 91;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 92;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 93;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 94;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 95;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 96;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 97;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 98;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 99;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 100;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 101;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 102;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 103;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 104;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 105;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 106;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 107;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 108;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 109;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 110;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 111;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 112;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 113;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 114;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 115;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 116;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 117;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 118;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 119;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 120;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 121;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 122;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 123;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 124;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 125;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 126;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 127;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 128;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 129;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 130;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 131;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 132;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 133;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 134;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 135;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 136;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 137;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 138;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 139;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 140;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 141;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 142;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 143;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 144;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 145;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 146;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 147;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 148;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 149;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 150;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 151;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 152;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 153;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 154;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 155;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 156;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 157;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 158;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 159;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 160;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 161;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 162;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 163;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 164;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 165;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 166;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 167;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 168;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 169;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 170;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 171;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 172;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 173;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 174;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 175;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 176;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 177;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 178;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 179;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 180;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 181;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 182;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 183;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 184;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 185;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 186;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 187;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 188;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
WHERE [Id] = 189;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
WHERE [Id] = 190;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
WHERE [Id] = 191;
SELECT @@ROWCOUNT;

GO

UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
WHERE [Id] = 192;
SELECT @@ROWCOUNT;

GO

CREATE INDEX [IX_AccreditationFees_TonnageBandId] ON [Lookup].[AccreditationFees] ([TonnageBandId]);
GO

ALTER TABLE [Lookup].[AccreditationFees] ADD CONSTRAINT [FK_AccreditationFees_TonnageBand_TonnageBandId] FOREIGN KEY ([TonnageBandId]) REFERENCES [Lookup].[TonnageBand] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250606151503_AccreditationFeeTableDesignChanges', N'8.0.4');
GO

COMMIT;
GO

