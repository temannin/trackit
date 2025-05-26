import { useState } from "react";

export default function PythonRunner() {
  const [code, setCode] = useState("");
  const [output, setOutput] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  async function handleRun(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);
    setOutput("");
    setError("");
    try {
      const res = await fetch("/api/python/run", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ code }),
      });
      const data = await res.json();
      setOutput(data.output);
      setError(data.error);
    } catch (err) {
      setError("Failed to run code");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="max-w-xl mx-auto mt-10 p-6 bg-white rounded-lg shadow">
      <h2 className="text-2xl font-bold mb-4 text-center">Run Python Code in Docker</h2>
      <form onSubmit={handleRun} className="flex flex-col gap-4">
        <textarea
          className="border rounded p-2 font-mono min-h-[150px]"
          value={code}
          onChange={e => setCode(e.target.value)}
          placeholder="Write your Python code here..."
          required
        />
        <button
          type="submit"
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 disabled:opacity-50"
          disabled={loading}
        >
          {loading ? "Running..." : "Run in Docker"}
        </button>
      </form>
      {(output || error) && (
        <div className="mt-6">
          <h3 className="font-semibold">Result:</h3>
          {output && (
            <pre className="bg-gray-100 p-2 rounded text-green-800 whitespace-pre-wrap">{output}</pre>
          )}
          {error && (
            <pre className="bg-gray-100 p-2 rounded text-red-800 whitespace-pre-wrap">{error}</pre>
          )}
        </div>
      )}
    </div>
  );
}
