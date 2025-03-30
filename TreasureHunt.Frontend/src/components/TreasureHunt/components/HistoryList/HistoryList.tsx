import React from 'react';
import {
  Card,
  CardContent,
  Typography,
  List,
  ListItem,
  ListItemText,
  Divider
} from '@mui/material';
import HistoryIcon from '@mui/icons-material/History';
import { historyListStyles } from './styles';
import { SavedTest } from '../../types';

interface HistoryListProps {
  tests: SavedTest[];
  onTestSelect: (test: SavedTest) => void;
}

const HistoryList: React.FC<HistoryListProps> = ({
  tests,
  onTestSelect
}) => {
  return (
    <Card elevation={3} sx={historyListStyles.card}>
      <CardContent>
        <Typography 
          variant="h6" 
          gutterBottom 
          sx={historyListStyles.title}
        >
          <HistoryIcon color="primary" />
          Histories
        </Typography>
        <List>
          {tests.map((test, index) => (
            <React.Fragment key={test.id}>
              <ListItem
                onClick={() => onTestSelect(test)}
                sx={historyListStyles.listItem}
              >
                <ListItemText
                  primary={
                    <Typography variant="subtitle1" sx={historyListStyles.testTitle}>
                      History #{test.id}
                    </Typography>
                  }
                  secondary={
                    <Typography variant="body2" color="text.secondary">
                      Matrix: {test.n}×{test.m} | Chests: {test.p} | Result: {test.result.toFixed(5)}
                      <br />
                      Created: {new Date(test.createdAt).toLocaleString()}
                    </Typography>
                  }
                />
              </ListItem>
              {index < tests.length - 1 && <Divider />}
            </React.Fragment>
          ))}
        </List>
      </CardContent>
    </Card>
  );
};

export default HistoryList; 