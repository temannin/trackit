// Utility function to POST data to the backend

export async function runPythonApi(data: any) {
  const response = await fetch("http://127.0.0.1:5062/api/python/run", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    // Handle error response
    throw new Error(`Error: ${response.statusText}`);
  }

  // Parse and return JSON response
  return await response.json();
}
