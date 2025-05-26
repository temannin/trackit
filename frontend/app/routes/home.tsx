import PythonRunner from "./python";

export function meta() {
  return [
    { title: "Python Docker Runner" },
    { name: "description", content: "Run custom Python code in Docker from a React UI." },
  ];
}

export default function Home() {
  return (
    <>
      <PythonRunner />
    </>
  );
}
