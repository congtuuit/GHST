const fs = require('fs-extra'); // Make sure to install fs-extra
const path = require('path');

// Get the source and target folders from command line arguments
const [sourceFolder, targetFolder] = process.argv.slice(2);

if (!sourceFolder || !targetFolder) {
  console.error('Please provide both source and target folder names.');
  process.exit(1); // Exit with error
}

// Construct full paths
const source = path.join(__dirname, sourceFolder);
const destination = path.resolve(targetFolder); // Ensure it's an absolute path

fs.copy(source, destination, { overwrite: true })
  .then(() => console.log('Move completed!'))
  .catch(err => console.error('Error:', err));
