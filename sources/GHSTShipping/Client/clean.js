const fs = require('fs');
const path = require('path');

// Get folder path from command-line arguments
const folderPath = process.argv[2];

if (!folderPath) {
    console.error('Please provide a folder path');
    process.exit(1);
}

function deleteFilesInFolder(folderPath) {
    fs.readdir(folderPath, (err, files) => {
        if (err) {
            console.error('Error reading the directory:', err);
            return;
        }

        files.forEach(file => {
            const filePath = path.join(folderPath, file);

            fs.unlink(filePath, (err) => {
                if (err) {
                    console.error('Error deleting file:', err);
                } else {
                    console.log(`Deleted file: ${filePath}`);
                }
            });
        });
    });
}

deleteFilesInFolder(folderPath);
