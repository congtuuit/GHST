// Function to build query string from object
export const buildQueryString = (params: Record<string, any>): string => {
  return Object.keys(params)
    .map(key => {
      const value = params[key];
      return `${encodeURIComponent(key)}=${encodeURIComponent(value)}`;
    })
    .join('&');
};
