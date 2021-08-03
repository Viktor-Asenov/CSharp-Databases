BACKUP DATABASE SoftUni TO DISK = 'This PC:\Downloads\Softuni-backup.bak'

USE SoftUni

DROP DATABASE SoftUni

RESTORE DATABASE SoftUni FROM DISK = 'This PC:\Downloads\Softuni-backup.bak';
