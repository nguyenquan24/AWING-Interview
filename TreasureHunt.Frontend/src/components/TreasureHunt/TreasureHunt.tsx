import React from 'react';
import {
  TextField,
  Button,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
  Box,
  Stack,
  Alert,
  IconButton,
  List,
  ListItem,
  ListItemText,
  Divider,
  Container,
  Card,
  CardContent,
  Tooltip,
  Fade,
  Zoom,
  useTheme
} from '@mui/material';
import PlayArrowIcon from '@mui/icons-material/PlayArrow';
import HistoryIcon from '@mui/icons-material/History';
import ChestIcon from '@mui/icons-material/Lock';
import axios from 'axios';
import { Header } from './components/Header';
import { InputForm } from './components/InputForm';
import { MatrixTable } from './components/MatrixTable';
import { ResultDisplay } from './components/ResultDisplay';
import { HistoryList } from './components/HistoryList';
import { SavedTest } from './types';

interface TreasureHuntProps {}

const TreasureHunt = () => {
  const theme = useTheme();
  const [n, setN] = React.useState<number>(3);
  const [m, setM] = React.useState<number>(3);
  const [p, setP] = React.useState<number>(3);
  const [matrix, setMatrix] = React.useState<number[][]>([]);
  const [result, setResult] = React.useState<number | null>(null);
  const [error, setError] = React.useState<string | null>(null);
  const [savedTests, setSavedTests] = React.useState<SavedTest[]>([]);

  const API_BASE_URL = process.env.REACT_APP_API_URL 
    ? `${process.env.REACT_APP_API_URL}/api/treasurehunt`
    : '/api/treasurehunt';

  const handleNChange = (newN: number) => {
    setN(newN);
    const newMatrix = Array(newN).fill(0).map(() => Array(m).fill(0));
    setMatrix(newMatrix);
    setResult(null);
    setError(null);
  };

  const handleMChange = (newM: number) => {
    setM(newM);
    const newMatrix = Array(n).fill(0).map(() => Array(newM).fill(0));
    setMatrix(newMatrix);
    setResult(null);
    setError(null);
  };

  const handlePChange = (newP: number) => {
    setP(newP);
    setResult(null);
    setError(null);
  };

  React.useEffect(() => {
    const initialMatrix = Array(n).fill(0).map(() => Array(m).fill(0));
    setMatrix(initialMatrix);
  }, []);

  const loadSavedTests = async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}`);
      setSavedTests(response.data);
    } catch (error) {
      console.error('Error loading saved tests:', error);
    }
  };

  React.useEffect(() => {
    loadSavedTests();
  }, []);

  const handleMatrixChange = (row: number, col: number, value: string) => {
    const newMatrix = [...matrix];
    newMatrix[row] = [...newMatrix[row]];
    newMatrix[row][col] = Number(value) || 0;
    setMatrix(newMatrix);
    setError(null);
  };

  const validateMatrix = (): boolean => {
    const hasInvalidValues = matrix.some(row => 
      row.some(cell => cell < 1 || cell > p)
    );
    if (hasInvalidValues) {
      setError('All values must be between 1 and ' + p);
      return false;
    }

    const pCount = matrix.flat().filter(cell => cell === p).length;
    if (pCount !== 1) {
      setError(`There must be exactly one treasure chest with value ${p}`);
      return false;
    }

    setError(null);
    return true;
  };

  const convertToMatrix2D = (jaggedArray: number[][]): { [key: string]: number } => {
    const result: { [key: string]: number } = {};
    for (let i = 0; i < jaggedArray.length; i++) {
      for (let j = 0; j < jaggedArray[i].length; j++) {
        result[`${i},${j}`] = jaggedArray[i][j];
      }
    }
    return result;
  };

  const handleSubmit = async () => {
    if (!validateMatrix()) return;

    try {
      const response = await axios.post(`${API_BASE_URL}/solve`, {
        n,
        m,
        p,
        matrix: convertToMatrix2D(matrix)
      });
      setResult(response.data);

      await loadSavedTests();
    } catch (error: any) {
      console.error('Error:', error);
      setError(error.response?.data || 'Failed to solve the puzzle. Please check your input.');
    }
  };

  const loadTest = (test: SavedTest) => {
    setN(test.n);
    setM(test.m);
    setP(test.p);
    try {
      const parsedMatrix = JSON.parse(test.matrixData);
      const newMatrix = Array(test.n).fill(0).map(() => Array(test.m).fill(0));
      Object.entries(parsedMatrix).forEach(([key, value]) => {
        const [i, j] = key.split(',').map(Number);
        newMatrix[i][j] = value as number;
      });
      setMatrix(newMatrix);
      setResult(test.result);
      setError(null);
    } catch (error) {
      console.error('Error parsing matrix:', error);
      setError('Failed to load the test case matrix.');
    }
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Fade in timeout={1000}>
        <Stack spacing={4}>
          <Header />

          <InputForm
            n={n}
            m={m}
            p={p}
            onChangeN={handleNChange}
            onChangeM={handleMChange}
            onChangeP={handlePChange}
          />

          {error && (
            <Zoom in>
              <Alert 
                severity="error" 
                variant="filled"
                sx={{ 
                  borderRadius: 2,
                  '& .MuiAlert-message': { fontSize: '1rem' }
                }}
              >
                {error}
              </Alert>
            </Zoom>
          )}

          <MatrixTable
            matrix={matrix}
            maxValue={p}
            onCellChange={handleMatrixChange}
          />

          <Stack direction="row" spacing={3} justifyContent="center">
            <Button 
              variant="contained" 
              color="primary" 
              onClick={handleSubmit}
              startIcon={<PlayArrowIcon />}
              size="medium"
              sx={{ 
                px: 3,
                py: 1,
                borderRadius: 2,
                boxShadow: theme.shadows[2],
                minWidth: '120px'
              }}
            >
              Solve
            </Button>
          </Stack>

          {result !== null && (
            <ResultDisplay result={result} />
          )}

          {savedTests.length > 0 && (
            <HistoryList
              tests={savedTests}
              onTestSelect={loadTest}
            />
          )}
        </Stack>
      </Fade>
    </Container>
  );
};

export default TreasureHunt as React.FC<TreasureHuntProps>;