interface ColumnType {
  key: string;
  code: string;
  registerDate: string;
  shopName: string;
  fullName: string;
  avgMonthlyCapacity: number;
  status: string;
}

// Helper function to generate random dates
function getRandomDate(start: Date, end: Date): string {
  const date = new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime()));
  return date.toISOString().split('T')[0]; // Return date in 'YYYY-MM-DD' format
}

// Function to generate 30 data entries
export const generateData = (count: number): ColumnType[] => {
  const statusOptions = ['Active', 'Inactive', 'Pending'];
  const data: ColumnType[] = [];

  for (let i = 1; i <= count; i++) {
    data.push({
      key: `key-${i}`,
      code: `C-${1000 + i}`, // Generates codes like C-1001, C-1002, etc.
      registerDate: getRandomDate(new Date(2020, 0, 1), new Date()), // Random date from 2020 to now
      shopName: `Shop ${i}`,
      fullName: `Full Name ${i}`,
      avgMonthlyCapacity: Math.floor(Math.random() * 5000) + 100, // Random capacity between 100 and 5000
      status: statusOptions[Math.floor(Math.random() * statusOptions.length)], // Random status
    });
  }

  return data;
};
