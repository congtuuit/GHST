const fs = require('fs');
const path = require('path');

// Function to generate a version from the current date and time
function generateVersionFromDate() {
  const now = new Date();
  const day = String(now.getDate()).padStart(2, '0');
  const month = String(now.getMonth() + 1).padStart(2, '0'); // Months are 0-based
  const year = String(now.getFullYear()).slice(2); // Get last 2 digits of the year
  const hours = String(now.getHours()).padStart(2, '0');
  const minutes = String(now.getMinutes()).padStart(2, '0');

  return `${day}${month}${year}${hours}${minutes}`;
}

// Function to update or add VITE_APP_VERSION in the .env.production file
function updateEnvVersion(version) {
  const envPath = path.resolve(__dirname, '.env.production');

  // Read the file content
  let envContent;
  try {
    envContent = fs.readFileSync(envPath, 'utf8');
  } catch (err) {
    console.error(`Error reading file: ${envPath}`, err);
    return;
  }

  // Define the pattern to match "VITE_APP_VERSION="
  const versionPattern = /^VITE_APP_VERSION=\d*/m;

  // Check if the pattern exists in the file
  if (versionPattern.test(envContent)) {
    // Replace the existing version with the new version
    envContent = envContent.replace(versionPattern, `VITE_APP_VERSION=${version}`);
  } else {
    // If the pattern doesn't exist, append it to the file
    envContent += `\nVITE_APP_VERSION=${version}`;
  }

  // Write the updated content back to the file
  try {
    fs.writeFileSync(envPath, envContent, 'utf8');
    console.log(`.env.production updated successfully with VITE_APP_VERSION=${version}`);
  } catch (err) {
    console.error(`Error writing to file: ${envPath}`, err);
  }
}

// Get the version from the command line arguments or generate one
//const userProvidedVersion = process.argv[2];
const version = generateVersionFromDate();

// Run the update
updateEnvVersion(version);
