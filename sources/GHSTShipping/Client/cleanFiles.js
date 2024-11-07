const fs = require('fs');
const path = require('path');

// Get the directory path from the command-line arguments
const targetDirectory = process.argv[2];

if (!targetDirectory) {
  console.error('Please provide a target directory.');
  process.exit(1);
}

async function deleteFiles() {
  try {
    // Read the file clearn.txt
    const data = fs.readFileSync('clearn.txt', 'utf8');
    // Split into lines, trim each line, and filter out empty lines
    const fileNames = data.split('\n').map(line => line.trim()).filter(Boolean);

    for (const fileName of fileNames) {
      const filePath = path.join(targetDirectory, fileName); // Build the full path

      try {
        // Check if the file exists at the constructed path
        if (fs.existsSync(filePath)) {
          // Delete the file
          await fs.promises.unlink(filePath);
          console.log(`Deleted file: ${filePath}`);
        } else {
          console.log(`File not found: ${filePath}`);
        }
      } catch (error) {
        console.error(`Could not delete file ${filePath}:`, error);
      }
    }

    console.log('File deletion completed.');
  } catch (error) {
    console.error('Error reading clearn.txt:', error);
  }
}

// Run the delete function
deleteFiles();
