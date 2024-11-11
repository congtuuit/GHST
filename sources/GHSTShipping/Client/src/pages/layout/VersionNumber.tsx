import React from 'react';

const VersionNumber = () => {
  // Access the environment variable
  const version = import.meta.env.VITE_APP_VERSION;

  return <div style={{ color: '#ffe6e780', position: 'absolute', bottom: 0, left: 2 }}>v{version || 'Not Available'}</div>;
};

export default VersionNumber;
