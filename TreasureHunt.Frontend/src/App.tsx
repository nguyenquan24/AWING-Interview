import React from 'react';
import { Container, CssBaseline, AppBar, Toolbar, Typography } from '@mui/material';
import TreasureHunt from './components/TreasureHunt/TreasureHunt';

function App() {
  return (
    <>
      <CssBaseline />
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Treasure Hunt Solver
          </Typography>
        </Toolbar>
      </AppBar>
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <TreasureHunt />
      </Container>
    </>
  );
}

export default App;