const fs = require('fs');
const path = require('path');
const archiver = require('archiver');
const { generateVersionFromDate } = require('./update-release-version');

// Get the directory path and options from the command-line arguments
const args = process.argv.slice(2);
const targetDirectory = args[0];
let outputPath = null;

// Helper function to format the current date as DDMMYYYY
function getCurrentDateFormatted() {
  const now = new Date();
  const day = String(now.getDate()).padStart(2, '0');
  const month = String(now.getMonth() + 1).padStart(2, '0'); // Months are 0-based
  const year = String(now.getFullYear()).slice(2); // Get last 2 digits of the year
  const hours = String(now.getHours()).padStart(2, '0');
  const minutes = String(now.getMinutes()).padStart(2, '0');

  return `${year}${month}${day}_${hours}${minutes}`;
}

// Helper function to parse command-line arguments
function parseArgs() {
  args.forEach((arg, index) => {
    if (arg === '-o' && args[index + 1]) {
      outputPath = args[index + 1];
    }
  });
}

// Run argument parsing
parseArgs();

if (!targetDirectory) {
  console.error('Please provide a target directory.');
  process.exit(1);
}

// Set the output file name and path
const outputFileName = `${getCurrentDateFormatted()}_${path.basename(targetDirectory)}_release.zip`;
outputPath = outputPath ? path.join(outputPath, outputFileName) : path.join(targetDirectory, outputFileName);

// Function to ensure the output directory exists
function ensureOutputDirectoryExists() {
  const dir = path.dirname(outputPath);
  if (!fs.existsSync(dir)) {
    console.log(`Directory doesn't exist, creating: ${dir}`);
    fs.mkdirSync(dir, { recursive: true });
  }
}

// Function to compress the directory
async function compressDirectory() {
  ensureOutputDirectoryExists(); // Make sure the output directory exists

  const output = fs.createWriteStream(outputPath);
  const archive = archiver('zip', { zlib: { level: 9 } }); // High compression level

  output.on('close', function () {
    console.log(`Created zip file: ${outputPath} (${archive.pointer()} total bytes)`);
  });

  archive.on('error', function (err) {
    throw err;
  });

  archive.pipe(output);

  // Append all files and folders in the target directory to the archive
  archive.directory(targetDirectory, false);

  await archive.finalize();
}

// Run the compress function
compressDirectory();
