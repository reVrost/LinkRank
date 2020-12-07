import React, { useState } from "react";
import styled from "styled-components";
import { SearchInput } from "./SearchInput";
import { Button } from "./Button";
import "./App.css";

const ContentBox = styled.div`
  align-self: center;
  display: flex;
  min-width: 32rem;
`;

const ResultBox = styled.div`
  margin-top: 1rem;
  border: 1px solid rgba(255, 255, 255, 0.12);
  display: block;
  overflow: hidden;
  border-radius: 4px;
`;

const serverURL = "https://localhost:5001";

const App = () => {
  const [ranks, setRanks] = useState([]);
  const [searchWord, setSearchWord] = useState("");
  const [searchUrl, setSearchUrl] = useState("");
  const [result, setResult] = useState({
    searchWord: "",
    searchUrl: "",
    rankings: [],
    error: "",
  });

  const handleRankMeClick = () => {
    fetch(`${serverURL}/api/linkrank/fetch-rank`, {
      method: "POST",
      headers: new Headers({ "content-type": "application/json" }),
      body: JSON.stringify({
        searchWord,
        searchUrl,
      }),
    })
      .then((x) => x.json())
      .then((res) => setResult(res));
  };

  const renderResultText = () => {
    return result.rankings.length === 0 ? (
      <p>
        <strong>{result.searchUrl}</strong> did not appear in the first 100
        search results on google with keyword{" "}
        <strong>'{result.searchWord}'</strong>
      </p>
    ) : (
      <p>
        <strong>{result.searchUrl}</strong> are ranked [
        {result.rankings.join(", ")}] with keyword{" "}
        <strong>'{result.searchWord}'</strong> on google search!
      </p>
    );
  };

  return (
    <div className="App">
      <ContentBox>
        <SearchInput
          placeholder="Word"
          onChange={(e) => {
            setSearchWord(e.currentTarget.value);
          }}
        />
        <SearchInput
          placeholder="Url"
          onChange={(e) => {
            setSearchUrl(e.currentTarget.value);
          }}
        />
        <Button onClick={handleRankMeClick}> Rank me! </Button>
      </ContentBox>
      <ResultBox>
        {result.searchWord && result.searchUrl && renderResultText()}
        {result.error && <p>{result.error}</p>}
      </ResultBox>
    </div>
  );
};

export default App;
