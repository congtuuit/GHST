const fs = require('fs');
const path = require('path');
const archiver = require('archiver');

// Get the directory path and optional output file name from the command-line arguments
const targetDirectory = process.argv[2];
const outputFileName = `${getCurrentDateFormatted()}_${process.argv[3]}` || `${getCurrentDateFormatted()}_release.zip`;

if (!targetDirectory) {
  console.error('Please provide a target directory.');
  process.exit(1);
}

// Helper function to format the current date as DDMMYYYY
function getCurrentDateFormatted() {
  const now = new Date();
  const day = String(now.getDate()).padStart(2, '0');
  const month = String(now.getMonth() + 1).padStart(2, '0'); // Months are zero-indexed
  const year = now.getFullYear();
  return `${day}${month}${year}`;
}

// Function to compress the directory
async function compressDirectory() {
  // Create the output path in the same directory as the target folder
  const outputPath = path.join(targetDirectory, outputFileName);
  const output = fs.createWriteStream(outputPath);
  const archive = archiver('zip', { zlib: { level: 9 } }); // High compression level

  output.on('close', function () {
    console.log(`Created zip file: ${outputPath} (${archive.pointer()} total bytes)`);
  });

  archive.on('error', function(err) {
    throw err;
  });

  archive.pipe(output);

  // Append all files and folders in the target directory to the archive
  archive.directory(targetDirectory, false);

  await archive.finalize();
}

// Run the compress function
compressDirectory();
